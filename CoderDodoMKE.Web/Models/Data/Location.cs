using CoderDojoMKE.Models;
using CoderDojoMKE.Web.Models.API;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoderDojoMKE.Web.Models.Data
{
    public class Location : ModelBase, ILocation
    {
        [Key]
        public Guid LocationID { get; set; }        

        [Required]
        public string LocationName { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public string Address1 { get; set; }        
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneDisplay { get; set; }
        public PhoneType PhoneType { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public string StateProvince { get; set; }
        public string UnitNumber { get; set; }
    }
}
