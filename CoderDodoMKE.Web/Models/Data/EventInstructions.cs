using System;
using System.ComponentModel.DataAnnotations;


namespace CoderDojoMKE.Web.Models.Data
{
    public class EventInstructions
    {
        [Key]
        public Guid ID { get; set; }        
        public string Instructions { get; set; }
    }
}