
using System;

namespace CoderDojoMKE.Web.Models.Data
{
    public class Enrollee : Person
    {
        public Guid EnrollerID { get; set; }
        public Enroller Enroller { get; set; }
    }
}
