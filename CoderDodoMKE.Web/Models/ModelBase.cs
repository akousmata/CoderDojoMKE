using CoderDojoMKE.Web.Models.API;
using System;

namespace CoderDojoMKE.Models
{
    public abstract class ModelBase : IAuditable
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
