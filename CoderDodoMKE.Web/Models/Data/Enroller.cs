using System.Collections.Generic;

namespace CoderDojoMKE.Web.Models.Data
{
    public class Enroller : Person
    {
        public virtual ICollection<Enrollee> Enrollees { get; set; }
    }
}
