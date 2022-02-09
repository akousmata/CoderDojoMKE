using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoderDojoMKE.Web.Models.Data
{
    public class Enrollment
    {
        [Key]
        public Guid EnrollmentID { get; set; }

        public Guid EventID { get; set; }
        public Event Event { get; set; }

        public Enrollee Enrollee { get; set; }
        public Enroller Enroller { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentResolution EnrollmentResolution { get; set; }
    }
}
