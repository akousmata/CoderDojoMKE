using CoderDojoMKE.Models;
using CoderDojoMKE.Web.Models.Auth;
using CoderDojoMKE.Web.Models.Data;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CoderDojoMKE.Web.Controllers
{
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]        
        public ActionResult Cancel(Guid id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var enrollment = _context.EnrollmentSet.SingleOrDefault(enr => enr.EnrollmentID == id && enr.Enroller.PersonID == userId);

            if (enrollment == null) throw new AccessViolationException(string.Format("Could not cancel enrollment ID {0} for user {1}", id, userId));

            var resolution = new EnrollmentResolution
            {
                Enrollment = enrollment,
                EnrollmentID = enrollment.EnrollmentID,
                EnrollmentResolutionID = Guid.NewGuid(),
                ResolutionDate = DateTime.Now,
                ResolutionResult = ResolutionResultType.Canceled
            };

            _context.EnrollmentResolutionSet.Add(resolution);
            _context.SaveChanges();
            return Content("Cancellation Successful");
        }
    }
}