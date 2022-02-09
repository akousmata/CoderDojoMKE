using CoderDojoMKE.Models;

namespace CoderDojoMKE.Web.Models.API
{
    public interface IContactable
    {
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string Country { get; set; }
        string Email { get; set; }
        string Phone { get; set; }        
        PhoneType PhoneType { get; set; }
        string PostalCode { get; set; }
        string Region { get; set; }
        string StateProvince { get; set; }
        string UnitNumber { get; set; }
    }
}
