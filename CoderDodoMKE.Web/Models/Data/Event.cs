using CoderDojoMKE.Models;
using CoderDojoMKE.Web.Models.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoderDojoMKE.Web.Models.Data
{
    public class Event : ModelBase, IEvent
    {
        [Key]
        public Guid EventID { get; set; }
        public string EventName { get; set; }
        public int MaximumSignUps { get; set; }
        public string ImageName { get; set; }
        public DateTime EventDateTime { get; set; }
        public DateTime SignUpStart { get; set; }

        public Guid EventInstructionsID { get; set; }
        [DisplayName("Instructions")]
        public virtual EventInstructions EventInstructions { get; set; }

        [DisplayName("Sign ups end")]
        public DateTime SignUpEnd { get; set; }

        [DisplayName("Registration")]
        public DateTime RegistrationStart { get; set; }
        public DateTime RegistrationEnd { get; set; }

        [DisplayName("Description")]
        public string EventDescription { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
