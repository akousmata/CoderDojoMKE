using CoderDojoMKE.Web.Models.Auth;
using System;
using System.Linq;
using System.Data.Entity;
using CoderDojoMKE.Web.Models.View;

namespace CoderDojoMKE.Web.Models.Data
{
    public class EventManager
    {
        private readonly ApplicationDbContext _context;
        public EventManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public Event GetEventNonResolvedEnrollments(Guid eventID)
        {
            var evt = (from e in _context.EventSet                                    
                                    .Include(e => e.Locations)
                                    .Include(e => e.EventInstructions)
                                    .Where(e => e.EventID == eventID)
                       select  e).ToList().FirstOrDefault();

            evt.Enrollments = (from enr in _context.EnrollmentSet
                                            .Include(enrollment => enrollment.EnrollmentResolution)
                               where enr.EnrollmentResolution.Equals(null) && enr.EventID == eventID
                               select enr).ToList();

            return evt;
        }    
        
        public Event Create(CreateEventViewModel model, string user)
        {
            var evt = new Event
            {
                CreatedBy = user,
                CreatedOn = DateTime.Now,
                EventDateTime = model.EventDateTime,
                EventDescription = model.EventDescription,
                EventID = Guid.NewGuid(),
                EventInstructionsID = model.EventInstructionsID,
                EventName = model.EventName,
                ImageName = model.ImageName,
                MaximumSignUps = model.MaximumSignUps,
                RegistrationEnd = model.RegistrationEnd,
                RegistrationStart = model.RegistrationStart,
                SignUpEnd = model.SignUpEnd,
                SignUpStart = model.SignUpStart
            };

            _context.EventSet.Add(evt);
            _context.SaveChanges();
            return evt;
        }

        public EventInstructions CreateInstructions(string instructions)
        {
            var evtInstructions = new EventInstructions
            {
                ID = Guid.NewGuid(),
                Instructions = instructions
            };
            _context.EventInstructionsSet.Add(evtInstructions);
            _context.SaveChanges();
            return evtInstructions;
        }
    }
}