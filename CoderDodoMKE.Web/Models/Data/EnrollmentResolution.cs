using CoderDojoMKE.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace CoderDojoMKE.Web.Models.Data
{
    public class EnrollmentResolution
    {
        [Key]
        public Guid EnrollmentResolutionID { get; set; }
        
        public Guid EnrollmentID { get; set; }      
        public Enrollment Enrollment { get; set; }

        [Required]
        public DateTime ResolutionDate { get; set; }

        [Required]
        public ResolutionResultType ResolutionResult { get; set; }
    }
}
