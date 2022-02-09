using CoderDojoMKE.Web.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace CoderDojoMKE.Web.Models.Data
{
    public class EnrollmentManager
    {
        private readonly ApplicationDbContext _context;
        public EnrollmentManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Enrollment> GetNonResolvedUserEnrollments(Guid userId)
        {
            var sixtyDaysAgo = DateTime.Now.AddDays(-60);
            var enrollments = (from enr in _context.EnrollmentSet
                                                .Include(e => e.Enrollee)
                                                .Include(e => e.Enroller)
                                                .Include(e => e.Event)
                                                .Where(e => e.Enroller.PersonID == userId && e.EnrollmentDate >= sixtyDaysAgo)
                                                .OrderByDescending(e => e.EnrollmentDate)
                               where !((from res in _context.EnrollmentResolutionSet.Where(ers => ers.EnrollmentID == enr.EnrollmentID) select res).Any())
                               select enr).ToList();
            return enrollments;
        }

        public IEnumerable<Enrollment> GetNonResolvedEventEnrollments(Guid eventID)
        {   
            var enrollments = (from enr in _context.EnrollmentSet
                                                .Include(e => e.Enrollee)
                                                .Include(e => e.Enroller)
                                                .Include(e => e.Event)
                                                .Where(e => e.EventID == eventID)
                                                .OrderByDescending(e => e.EnrollmentDate)
                               where !((from res in _context.EnrollmentResolutionSet.Where(ers => ers.EnrollmentID == enr.EnrollmentID) select res).Any())
                               select enr).ToList();
            return enrollments;
        }        

        public IEnumerable<Enrollment> GetRecentUserEnrollments(Guid userID)
        {
            var enrollments = (from enr in _context.EnrollmentSet
                                                .Include(e => e.Enrollee)
                                                .Include(e => e.Enroller)
                                                .Include(e => e.Event)
                                                .Where(e => e.Enroller.PersonID == userID)
                                                .OrderByDescending(e => e.EnrollmentDate)
                                                            
                               select enr);

            List<Enrollment> recentEnrollments = new List<Enrollment>();
            if(enrollments.Any())
            {
                // Find the most recent set of enrollments where the event ID's match
                var firstEnrollmentEventID = enrollments.First().EventID;

                var firstEnrollments = enrollments.Where(enr => enr.EventID == firstEnrollmentEventID);
                foreach(var enr in firstEnrollments)
                {
                    if(recentEnrollments.All(re => re.Enrollee.PersonID != enr.Enrollee.PersonID))
                    {
                        recentEnrollments.Add(enr);
                    }
                }
            }

            return recentEnrollments;
        }

        public IEnumerable<Enrollment> GetNonResolvedUserEventEnrollments(Guid userID, Guid eventID)
        {
            var enrollments = (from enr in _context.EnrollmentSet
                                                .Include(e => e.Enrollee)
                                                .Include(e => e.Enroller)
                                                .Include(e => e.Event)
                                                .Where(e => e.Enroller.PersonID == userID && e.EventID == eventID)
                                                .OrderByDescending(e => e.EnrollmentDate)
                               where !((from res in _context.EnrollmentResolutionSet.Where(ers => ers.EnrollmentID == enr.EnrollmentID) select res).Any())
                               select enr).ToList();
            return enrollments;
        }

        public IEnumerable<EnrollmentResolution> GetUserEnrollmentResolutions(Guid userID)
        {
            var resolutions = (from enr in _context.EnrollmentResolutionSet
                                                .Include(e => e.Enrollment)
                                                .Include(e => e.Enrollment.Enroller)                                                
                                                .Where(e => e.Enrollment.Enroller.PersonID == userID)                                                                               
                               select enr).ToList();
            return resolutions;
        }

        public IEnumerable<Enrollment> GetAllEventEnrollments(Guid eventID)
        {
            var enrollments = (from enr in _context.EnrollmentSet
                                                .Include(e => e.Enrollee)
                                                .Include(e => e.Enroller)
                                                .Include(e => e.Event)
                                                .Include(e => e.EnrollmentResolution)
                                                .Where(e => e.EventID == eventID)
                                                .OrderByDescending(e => e.EnrollmentDate)
                               select enr).ToList();
            return enrollments;
        }
    }
}
