using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoderDojoMKE.Web.Models.API
{
    public interface IEvent : IAuditable
    {
        Guid EventID { get; set; }
        string EventName { get; set; }
        DateTime EventDateTime { get; set; }
        DateTime SignUpStart { get; set; }
        DateTime SignUpEnd { get; set; }
        DateTime RegistrationStart { get; set; }
        DateTime RegistrationEnd { get; set; }
    }
}
