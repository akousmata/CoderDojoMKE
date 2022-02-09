using AutoMapper;
using CoderDojoMKE.Models;
using CoderDojoMKE.Web.Models.Auth;
using CoderDojoMKE.Web.Models.Data;
using CoderDojoMKE.Web.Models.View;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CoderDojoMKE.Web.Controllers
{
    [Authorize(Roles = "Mentor,GlobalAdmin")]
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EnrollmentManager _enrollmentManager;

        public RegistrationController()
        {
            _context = new ApplicationDbContext();
            _enrollmentManager = new EnrollmentManager(_context);
        }

        public RegistrationController(ApplicationDbContext context)
        {
            _context = context;
            _enrollmentManager = new EnrollmentManager(context);
        }
        // GET: Registration
        public ActionResult Index()
        {
            // Get all the events that are happening within 48 hours of now
            var previousDay = DateTime.Now.AddHours(-72);
            IEnumerable<Event> events =
                _context.EventSet
                    .Where(e => e.RegistrationStart > previousDay)
                    .OrderBy(e => e.EventDateTime).ToList();

            var eventsModel = new List<EventViewModel>();
            foreach (var evt in events)
            {
                var evtViewModel = Mapper.Map<EventViewModel>(evt);
                evtViewModel.Enrollments = _enrollmentManager.GetNonResolvedEventEnrollments(evt.EventID).ToList();
                eventsModel.Add(evtViewModel);                
            }

            return View(eventsModel);            
        }

        public ActionResult Event(Guid id)
        {
            var evt = _context.EventSet.SingleOrDefault(e => e.EventID == id);
            var model = new EventRegistrationViewModel
            {
                EventID = evt.EventID,
                EventName = evt.EventName,
                Enrollments = _enrollmentManager.GetAllEventEnrollments(id)
            };
            
            return View("Event", model);
        }

        public ActionResult RegisterAttendee(Guid enrollmentID)
        {
            var enrollment = HandleEnrollmentResolution(enrollmentID, ResolutionResultType.InAttendance);
            return Event(enrollment.EventID);
        }

        public ActionResult RegisterCancellation(Guid enrollmentID)
        {
            var enrollment = HandleEnrollmentResolution(enrollmentID, ResolutionResultType.Canceled);
            return Event(enrollment.EventID);
        }

        public ActionResult RegisterNoShow(Guid enrollmentID)
        {
            var enrollment = HandleEnrollmentResolution(enrollmentID, ResolutionResultType.NoShowed);
            return Event(enrollment.EventID);
        }

        public ActionResult RegisterOther(Guid enrollmentID)
        {
            var enrollment = HandleEnrollmentResolution(enrollmentID, ResolutionResultType.Other);
            return Event(enrollment.EventID);
        }

        private Enrollment HandleEnrollmentResolution(Guid enrollmentID, ResolutionResultType resolutionResult)
        {
            var enrollment = _context.EnrollmentSet
                .Include(enr => enr.Enroller)
                .Include(enr => enr.Enrollee)
                .SingleOrDefault(enr => enr.EnrollmentID == enrollmentID);
            if (enrollment == null) throw new ArgumentNullException("enrollment", String.Format("The enrollment ID provided <{0}> does not correspond to an enrollment on the database", enrollmentID));

            var resolution = new EnrollmentResolution
            {
                Enrollment = enrollment,
                EnrollmentID = enrollment.EnrollmentID,
                EnrollmentResolutionID = Guid.NewGuid(),
                ResolutionDate = DateTime.Now,
                ResolutionResult = resolutionResult
            };

            _context.EnrollmentResolutionSet.Add(resolution);
            _context.SaveChanges();
            return enrollment;          
        }
    }
}