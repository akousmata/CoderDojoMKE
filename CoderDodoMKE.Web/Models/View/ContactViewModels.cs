using System.ComponentModel.DataAnnotations;

namespace CoderDojoMKE.Web.Models.View
{
    public class ContactUsInformationViewModel
    {
        [Required(ErrorMessage="A name is required")]
        public string ContactUsName { get; set; }

        [Required(ErrorMessage="An email is required")]
        public string ContactUsEmail { get; set; }

        [Required(ErrorMessage="A message is required")]
        public string ContactUsMessage { get; set; }
    }
}