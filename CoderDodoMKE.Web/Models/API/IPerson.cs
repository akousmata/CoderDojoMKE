using System;

namespace CoderDojoMKE.Web.Models.API
{
    public interface IPerson : IAuditable, IContactable
    {   
        Guid PersonID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string FullName { get; }
        string FullNameReverse { get; }        
    }
}
