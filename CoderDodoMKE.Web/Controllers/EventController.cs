using AutoMapper;
using CoderDojoMKE.Models;
using CoderDojoMKE.Web.Models.Auth;
using CoderDojoMKE.Web.Models.Data;
using CoderDojoMKE.Web.Models.View;
using CoderDojoMKE.Web.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoderDojoMKE.Web.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private int MaximumNoShows = 5;
        private readonly ApplicationDbContext _context;
        private readonly EnrollmentManager _enrollmentManager;
        private readonly EventManager _eventManager;
        private ApplicationUserManager _userManager;

        public EventController()
        {
            _context = new ApplicationDbContext();
            _enrollmentManager = new EnrollmentManager(_context);
            _eventManager = new EventManager(_context);
        }

        public EventController(ApplicationDbContext context)
        {
            _context = context;
            _enrollmentManager = new EnrollmentManager(context);
        }

        public EventController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
            _context = new ApplicationDbContext();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public async Task<ActionResult> Signup(Guid id)
        {
            // Check if this user has 3 or more no-shows and prevent them from
            // accessing the sign up
            var userID = Guid.Parse(User.Identity.GetUserId());
            var resolvedEnrollments = _enrollmentManager.GetUserEnrollmentResolutions(userID);
            if(resolvedEnrollments.Count(e => e.ResolutionResult == ResolutionResultType.NoShowed) >= MaximumNoShows)
            {
                return RedirectToAction("TooManyNoShows", "Signup");
            }

            Event evt = _eventManager.GetEventNonResolvedEnrollments(id);

            if (evt == null) return View("Error");
            var viewModel = Mapper.Map<EventSignupViewModel>(evt);

            // Check if this user has signed up before by getting the most recent signup           
            var enrollments = _enrollmentManager.GetRecentUserEnrollments(userID);
            var enrollees = _context.EnrolleeSet.Include(enr => enr.Enroller).Where(enr => enr.Enroller.PersonID == userID).ToList();
            if (enrollments.Any())
            {
                // They have a previous signup, check in the number of enrollees from the previous signup matches their
                // total number of enrollees.  If they match, simply display them as disabled, if they want to change it,
                // they can change the total number of enrollees in order to add or remove one.                
                viewModel.NumberOfEnrollees = enrollments.Count();
                foreach (var enrollee in enrollees)
                {
                    viewModel.SelectableEnrollees.Add(
                        new SelectListItem
                        {
                            Text = enrollee.FirstName + " " + enrollee.LastName,
                            Value = enrollee.PersonID.ToString(),
                            Disabled = enrollments.Count() == enrollees.Count()
                        });
                }
                viewModel.SelectableEnrollees.Add(new SelectListItem { Text = "Create new...", Value = new Guid().ToString() });

                // Pre-select their previous enrollments in the dropdowns
                foreach (var enrollment in enrollments)
                {
                    viewModel.SelectedEnrollees.Add(enrollment.Enrollee.PersonID);
                }
            }
            else
            {
                viewModel.IsFirstSignup = true;
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeAttendees(EventSignupViewModel model)
        {
            if (model == null) return View("Error");

            Event evt = _context.EventSet
                .Include(e => e.Enrollments)
                .Include(e => e.Locations)
                .Include(e => e.EventInstructions)
                .SingleOrDefault(e => e.EventID == model.EventID);

            if (evt == null) return View("Error");
            
            var viewModel = Mapper.Map<EventSignupViewModel>(evt);
            var userID = Guid.Parse(User.Identity.GetUserId());
            Enroller enroller = await _context.EnrollerSet.Include(es => es.Enrollees).SingleOrDefaultAsync(es => es.PersonID == userID);
            if (enroller == null) return View("Error");

            // All data checks out, it's now safe to proceed, if this is a first time signup, just display the table to the end user.      
            if (model.IsFirstSignup)
            {
                List<EventEnrolleeViewModel> enrollees = new List<EventEnrolleeViewModel>();
                for (int i = 0; i < model.NumberOfEnrollees; i++)
                {
                    var enrolleeViewModel = new EventEnrolleeViewModel();
                    enrolleeViewModel.PersonID = Guid.NewGuid();
                    enrolleeViewModel.EnrollerID = enroller.PersonID;
                    enrollees.Add(enrolleeViewModel);
                }

                viewModel.NumberOfEnrollees = model.NumberOfEnrollees;
                viewModel.Enrollees = enrollees;

                // Clearing the model state allows the user to quick change the dropdown seats without receiving error messages
                ModelState.Clear();
                return View("Signup", viewModel);
            }

            // We know it's not a first timer, we need to sync the possible options:            
            var enrollments = _enrollmentManager.GetRecentUserEnrollments(userID);

            // Pre-select enrollees in the dropdowns
            if (model.NumberOfEnrollees <= enrollments.Count())
            {
                // Use the enrollments as the source data for the dropdowns
                for (int i = 0; i < model.NumberOfEnrollees; i++)
                {
                    viewModel.SelectedEnrollees.Add(enrollments.ElementAt(i).Enrollee.PersonID);
                }
            }
            else
            {
                // Use the enrollees as the source data for the dropdowns
                var newEnrollees = new List<EventEnrolleeViewModel>();
                for (int i = 0; i < model.NumberOfEnrollees; i++)
                {
                    // We may not have as many enrollees as what the user wants to enroll, so 
                    // fall back to the table based layout for the overage.
                    var enrollee = enroller.Enrollees.ElementAtOrDefault(i);
                    if(enrollee != null)
                    {
                        viewModel.SelectedEnrollees.Add(enrollee.PersonID);
                    } 
                    else
                    {
                        newEnrollees.Add(new EventEnrolleeViewModel
                            {
                                EnrollerID = enroller.PersonID,
                                PersonID = Guid.NewGuid()
                            });
                    }                    
                }

                viewModel.Enrollees = newEnrollees;
            }

            // Populate the selection options for the dropdowns            
            foreach (var enrollee in enroller.Enrollees)
            {
                viewModel.SelectableEnrollees.Add(
                    new SelectListItem
                    {
                        Text = enrollee.FullName,
                        Value = enrollee.PersonID.ToString()             
                    });
            }
            viewModel.SelectableEnrollees.Add(new SelectListItem { Text = "Create new...", Value = new Guid().ToString() });

            viewModel.NumberOfEnrollees = model.NumberOfEnrollees;
            ModelState.Clear();
            return View("Signup", viewModel);
        }        

        [HttpPost]
        public ActionResult CreateNewNinja(CreateNewNinjaViewModel model)
        {
            if(ModelState.IsValid)
            {
                var userID = Guid.Parse(User.Identity.GetUserId());
                var enrollee = new Enrollee
                {
                    FirstName = model.NewNinjaFirstName,
                    LastName = model.NewNinjaLastName,
                    EnrollerID = userID,
                    PersonID = Guid.NewGuid(),
                    CreatedBy = User.Identity.Name,
                    CreatedOn = DateTime.Now
                };
                var enroller = _context.EnrollerSet.Include(enr => enr.Enrollees).SingleOrDefault(enr => enr.PersonID == userID);
                enroller.Enrollees.Add(enrollee);
                _context.EnrolleeSet.Add(enrollee);
                _context.SaveChanges();
                return Json(new { Name = enrollee.FullName, ID = enrollee.PersonID, Selected = true });
            }

            return PartialView("_CreateNewNinjaInputs", model);
        }

        [HttpPost]
        public async Task<ActionResult> Enroll(EventSignupViewModel model)
        {
            if(ModelState.IsValid)
            {
                // Extract the user as an enroller
                var userID = Guid.Parse(User.Identity.GetUserId());
                Enroller enroller = await _context.EnrollerSet.Include(es => es.Enrollees).SingleOrDefaultAsync(es => es.PersonID == userID);
                if (enroller == null) return View("Error");

                // Extract the event they are signing up for
                Event evt = await _context.EventSet.SingleOrDefaultAsync(e => e.EventID == model.EventID);
                if (evt == null) return View("Error");

                // Handle first time sigup
                foreach (var enrollee in model.Enrollees)
                {
                    // Check if an insert needs to occur to create new enrollees
                    var dbEnrollee = await _context.EnrolleeSet.FindAsync(enrollee.PersonID);
                    if (dbEnrollee == null)
                    {
                        dbEnrollee = new Enrollee
                        {
                            CreatedBy = User.Identity.Name,
                            CreatedOn = DateTime.Now,
                            EnrollerID = enrollee.EnrollerID,
                            PersonID = enrollee.PersonID,
                            FirstName = enrollee.FirstName,
                            LastName = enrollee.LastName
                        };
                        _context.EnrolleeSet.Add(dbEnrollee);
                        enroller.Enrollees.Add(dbEnrollee);
                    }

                    // Add the enrollment for each enrollee to the database
                    var enrollment = new Enrollment
                    {
                        Enrollee = dbEnrollee,
                        Enroller = enroller,
                        EnrollmentDate = DateTime.Now,
                        EnrollmentID = Guid.NewGuid(),
                        Event = evt,
                        EventID = evt.EventID
                    };
                    _context.EnrollmentSet.Add(enrollment);
                }

                // Handle existing enrollees
                foreach(var id in model.SelectedEnrollees)
                {
                    // Add the enrollment for each enrollee to the database
                    var dbEnrollee = await _context.EnrolleeSet.FindAsync(id);
                    var enrollment = new Enrollment
                    {
                        Enrollee = dbEnrollee,
                        Enroller = enroller,
                        EnrollmentDate = DateTime.Now,
                        EnrollmentID = Guid.NewGuid(),
                        Event = evt,
                        EventID = evt.EventID
                    };
                    _context.EnrollmentSet.Add(enrollment);
                }

                await _context.SaveChangesAsync();
                int totalEnrollees = model.Enrollees.Count + model.SelectedEnrollees.Count;
                string emailMessage = GetEnrollmentMessage(model, evt);
                var message = MailHelper.GetMailMessage("Coder Dojo MKE", "info@coderdojomke.org", User.Identity.Name, "Enrollment Information", emailMessage);
                MailHelper.Send(message);
                return View("EnrollmentSuccess", new EnrollmentSuccessViewModel { EventDateTime = evt.EventDateTime, EventName = evt.EventName, NumberOfNinjas = totalEnrollees });
            }
            else
            {
                Event evt = _context.EventSet
                    .Include(e => e.Enrollments)
                    .Include(e => e.Locations)
                    .Include(e => e.EventInstructions)
                    .SingleOrDefault(e => e.EventID == model.EventID);

                if (evt == null) return View("Error");
                Mapper.Map(evt, model);
                return View("Signup", model);
            }
        }

        private string GetEnrollmentMessage(EventSignupViewModel model, Event evt)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("<p>Thank you for enrolling your ninja(s) for the Coder Dojo MKE Event <b>{0}</b></p>", evt.EventName));
            sb.AppendLine("<p>The following ninja(s) were enrolled:</p>");
            sb.AppendLine("<ul>");
            foreach(var enrollee in model.Enrollees)
            {
                sb.AppendLine(string.Format("<li>{0} {1}</li>", enrollee.FirstName, enrollee.LastName));
            }
            
            foreach(var id in model.SelectedEnrollees)
            {
                var enrollee = _context.EnrolleeSet.Find(id);
                if(enrollee != null)
                {
                    sb.AppendLine(String.Format("<li>{0}</li>", enrollee.FullName));
                }
            }
            sb.AppendLine("</ul>");

            sb.AppendLine(string.Format("<p>The event begins at <b>{0}</b> with registrations starting at <b>{1}</b>.<p>", evt.EventDateTime, evt.RegistrationStart));
            sb.AppendLine("<p>If you need to cancel any of your enrollments you can do so on the <a href=\"https://www.coderdojomke.org/Manage/Index\">account management page</a>");            
            sb.AppendLine("If there is less than 24 hours to the event and you need to cancel please contact us to let us know via e-mail.</p>");
            if(evt.EventInstructions != null && !string.IsNullOrWhiteSpace(evt.EventInstructions.Instructions))
            {
                sb.AppendLine("<p>This event contains the following instructions</p>");
                sb.AppendLine(string.Format("<p>{0}</p>", evt.EventInstructions.Instructions));
            }
            else
            {
                sb.AppendLine("<p>This event does not contain any special instructions.</p>");
            }
            sb.AppendLine("<p>Event Location</p>");
            sb.AppendLine("<address>1572 E. Capitol Dr.<br />Shorewood, WI 53211</address>");

            return sb.ToString();
        }

        public ActionResult EnrollmentSuccess()
        {
            return View(new EnrollmentSuccessViewModel());
        }
    }
}