using System;

namespace CoderDojoMKE.Web.Models.API
{
    public interface ILocation : IAuditable, IContactable
    {
        Guid LocationID { get; set; }
        string LocationName { get; set; }        
    }
}
