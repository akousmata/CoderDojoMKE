using CoderDojoMKE.Models;
using CoderDojoMKE.Web.Models.API;
using System;
using System.ComponentModel.DataAnnotations;

namespace CoderDojoMKE.Web.Models.Data
{
    public abstract class Person : ModelBase, IPerson
    {
        [Key]
        public Guid PersonID { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                string fullName = "";
                if(!(string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName)))
                {
                    fullName += FirstName + " " + LastName;
                }
                else if (string.IsNullOrWhiteSpace(FirstName))
                {
                    fullName += LastName;
                }
                else if(string.IsNullOrWhiteSpace(LastName))
                {
                    fullName += FirstName;
                }

                return fullName;
            }
        }

        public string FullNameReverse
        {
            get
            {
                string fullName = "";
                if (!(string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName)))
                {
                    fullName += LastName + ", " + FirstName;
                }
                else if (string.IsNullOrWhiteSpace(FirstName))
                {
                    fullName += LastName;
                }
                else if (string.IsNullOrWhiteSpace(LastName))
                {
                    fullName += FirstName;
                }

                return fullName;
            }
        }

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
