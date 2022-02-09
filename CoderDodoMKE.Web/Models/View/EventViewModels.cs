using CoderDojoMKE.Web.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace CoderDojoMKE.Web.Models.View
{
    public class EventViewModel
    {
        public Guid EventID { get; set; }
        public string EventName { get; set; }
        public int MaximumSignUps { get; set; }
        public string ImageName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDateTime { get; set; }
        public DateTime EventEndDateTime { get { return EventDateTime.AddHours(2); } }
        public DateTime SignUpStart { get; set; }
        
        [DisplayName("Instructions")]
        public EventInstructions EventInstructions { get; set; }

        [DisplayName("Sign ups end")]
        public DateTime SignUpEnd { get; set; }

        [DisplayName("Registration")]
        public DateTime RegistrationStart { get; set; }        
        
        [DisplayName("Seats Available")]
        public int SeatsAvailable
        {
            get
            {
                if (Enrollments == null) return 0;
                return MaximumSignUps - Enrollments.Count();
            }
        }
                
        public IEnumerable<Enrollment> Enrollments;
    }

    public class CreateEventViewModel
    {
        [Key]
        public Guid EventID { get; set; }

        [DisplayName("Event Name")]
        public string EventName { get; set; }

        [DisplayName("Max Sign Ups")]
        public int MaximumSignUps { get; set; }

        [DisplayName("Image")]
        public string ImageName { get; set; }

        [DisplayName("Date / Time")]
        public DateTime EventDateTime { get; set; }

        [DisplayName("Sign Up Start")]
        public DateTime SignUpStart { get; set; }

        [DisplayName("Instructions")]
        public Guid EventInstructionsID { get; set; }        
        public IEnumerable<SelectListItem> EventInstructions { get; set; }

        [DisplayName("Sign Up End")]
        public DateTime SignUpEnd { get; set; }

        [DisplayName("Registration Start")]
        public DateTime RegistrationStart { get; set; }

        [DisplayName("Registration End")]
        public DateTime RegistrationEnd { get; set; }

        [DisplayName("Description")]
        public string EventDescription { get; set; }
    }

    public class CreateEventInstructionsViewModel
    {
        public Guid ID { get; set; }
        public string Instructions { get; set; }
    }

    public class EventSignupViewModel
    {
        public EventSignupViewModel()
        {
            Enrollees = new List<EventEnrolleeViewModel>();
            SelectedEnrollees = new List<Guid>();
            SelectableEnrollees = new List<SelectListItem>();
        }

        public Guid EventID { get; set; }
        public string EventName { get; set; }
        public int MaximumSignUps { get; set; }
        public string ImageName { get; set; }
        public DateTime EventDateTime { get; set; }
        public DateTime EventEndDateTime { get { return EventDateTime.AddHours(2); } }
        public DateTime SignUpStart { get; set; }
        public bool IsFirstSignup { get; set; }
        public string EventDescription { get; set; }

        [DisplayName("Instructions")]
        public EventInstructions EventInstructions { get; set; }

        [DisplayName("Sign ups end")]
        public DateTime SignUpEnd { get; set; }

        [DisplayName("Registration")]
        public DateTime RegistrationStart { get; set; }

        [DisplayName("Seats Available")]
        public int SeatsAvailable
        {
            get
            {
                if (Enrollments == null) return 0;
                return MaximumSignUps - Enrollments.Count();
            }
        }        

        [DisplayName("Number of seats")]
        public int NumberOfEnrollees { get; set; }
        public IEnumerable<SelectListItem> GetNumberOfEnrolleesList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem {Text = "--", Value = "0", Selected = NumberOfEnrollees == 0},
                new SelectListItem {Text = "1", Value = "1", Selected = NumberOfEnrollees == 1 },
                new SelectListItem {Text = "2", Value = "2", Selected = NumberOfEnrollees == 2 },
                new SelectListItem {Text = "3", Value = "3", Selected = NumberOfEnrollees == 3 },
                new SelectListItem {Text = "4", Value = "4", Selected = NumberOfEnrollees == 4 },
                new SelectListItem {Text = "5", Value = "5", Selected = NumberOfEnrollees == 5 }
            };            
        }

        public IList<Guid> SelectedEnrollees { get; set; }
        public IList<SelectListItem> SelectableEnrollees { get; set; }
        
        
        public IList<EventEnrolleeViewModel> Enrollees { get; set; }
        public IList<Enrollment> Enrollments;
    }

    public class EventEnrolleeViewModel
    {
        [Required]
        public Guid PersonID { get; set; }
        [Required]
        public Guid EnrollerID { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
    }

    public class CreateNewNinjaViewModel
    {
        [Required]
        [DisplayName("First Name")]
        public string NewNinjaFirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string NewNinjaLastName { get; set; }
    }

    public class EnrollmentSuccessViewModel
    {
        public int NumberOfNinjas { get; set; }
        public string EventName { get; set; }
        public DateTime EventDateTime { get; set; }
    }

    public class EventRegistrationViewModel
    {
        public string EventName { get; set; }
        public Guid EventID { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}