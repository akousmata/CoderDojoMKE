using AutoMapper;
using CoderDojoMKE.Web.Models.Auth;
using CoderDojoMKE.Web.Models.Data;
using CoderDojoMKE.Web.Models.View;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CoderDojoMKE.Web.Controllers
{
    public class SignupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EnrollmentManager _enrollmentManager;

        public SignupController()
        {
            _context = new ApplicationDbContext();
            _enrollmentManager = new EnrollmentManager(_context);
        }

        public SignupController(ApplicationDbContext context)
        {
            _context = context;
            _enrollmentManager = new EnrollmentManager(_context);
        }
        
        public bool IsAuthenticated
        {
            get
            {
                if (User != null && User.Identity != null)
                {
                    return User.Identity.IsAuthenticated;
                }

                return false;
            }
        }  

        // GET: Signup        
        [AllowAnonymous]
        public ActionResult Index()
        {
            IEnumerable<Event> events = 
                _context.EventSet                                                          
                    .Where(e => e.SignUpEnd > DateTime.Now)
                    .OrderBy(e => e.EventDateTime).ToList();

            var eventsModel = new List<EventViewModel>();
            foreach (var evt in events)
            {
                if (IsAuthenticated)
                {
                    var userID = Guid.Parse(User.Identity.GetUserId());                
                    var userEnrollments = _enrollmentManager.GetNonResolvedUserEventEnrollments(userID, evt.EventID);

                    // If the user has people signed up for this event, they should not see it in the list.
                    // They will need to cancel all enrollments first before they can see it again.
                    if (userEnrollments.Count() == 0)
                    {
                        var evtViewModel = Mapper.Map<EventViewModel>(evt);
                        evtViewModel.Enrollments = _enrollmentManager.GetNonResolvedEventEnrollments(evt.EventID).ToList();
                        eventsModel.Add(evtViewModel);
                    }
                }
                else
                {
                    var evtViewModel = Mapper.Map<EventViewModel>(evt);
                    evtViewModel.Enrollments = _enrollmentManager.GetNonResolvedEventEnrollments(evt.EventID).ToList();
                    eventsModel.Add(evtViewModel);
                }
            }
            
            
            
            
            return View(eventsModel);
        }

        [AllowAnonymous]
        public ActionResult TooManyNoShows()
        {
            return View();
        }
    }
}