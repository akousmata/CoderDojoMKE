using CoderDojoMKE.Models;
using CoderDojoMKE.Web.Models.Auth;
using CoderDojoMKE.Web.Models.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace CoderDojoMKE.Web.Init
{
    public class CDMInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        #region Private tracking vars
        private EventInstructions htmlInstructions;
        private EventInstructions mineCraftInstructions;
        #endregion
        #region Lists for inserting        
        private List<Mentor> mentors = new List<Mentor>();
        private List<Enroller> enrollers = new List<Enroller>();
        private List<Enrollee> enrollees = new List<Enrollee>();        
        private List<Location> locations = new List<Location>();        
        private List<Enrollment> enrollments = new List<Enrollment>();
        private List<EnrollmentResolution> resolutions = new List<EnrollmentResolution>();
        #endregion

        protected override void Seed(ApplicationDbContext context)
        {
            SeedAuth(context);
            SeedApplication(context);
        }


        private void SeedAuth(ApplicationDbContext context)
        {
            RoleStore<ApplicationRole> roleStore = new RoleStore<ApplicationRole>(context);
            RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(roleStore);

            ApplicationRole role = new ApplicationRole("GlobalAdmin", "Global Access");
            IdentityResult idResult = roleManager.Create(role);

            ApplicationRole mentorRole = new ApplicationRole("Mentor", "Access to enrollment resolutions, enrollments, reports");
            IdentityResult mentorResult = roleManager.Create(mentorRole);

            ApplicationRole standardRole = new ApplicationRole("StandardUser", "Access to enroll an enrollee");
            IdentityResult standardResult = roleManager.Create(standardRole);

            // Create debug (testing) objects here
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            ApplicationUser mentor = new ApplicationUser { UserName = "mentor1@test1.com", Email = "mentor1@test1.com" };
            IdentityResult mentorUserResult = userManager.Create(mentor, "abc123");
            userManager.AddToRole(mentor.Id, "Mentor");

            ApplicationUser enroller1 = new ApplicationUser { UserName = "enroller1@test1.com", Email = "enroller1@test1.com" };
            IdentityResult enroller1Result = userManager.Create(enroller1, "abc123");
            userManager.AddToRole(enroller1.Id, "StandardUser");            

            ApplicationUser enroller2 = new ApplicationUser { UserName = "enroller2@test2.com", Email = "enroller2@test2.com" };
            IdentityResult enroller2Result = userManager.Create(enroller2, "abc123");
            userManager.AddToRole(enroller2.Id, "StandardUser");

            ApplicationUser enroller3 = new ApplicationUser { UserName = "enroller3@test3.com", Email = "enroller3@test3.com" };
            IdentityResult enroller3Result = userManager.Create(enroller3, "abc123");
            userManager.AddToRole(enroller3.Id, "StandardUser");
        }

        private void SeedApplication(ApplicationDbContext context)
        {
            // Set up all the people
            var mentors = new List<Mentor>();
            for (int i = 0; i < 5; i++)
            {
                mentors.Add(BuildNewMentor(i));
            }

            var enrollers = new List<Enroller>();
            for (int i = 0; i < 40; i++)
            {
                enrollers.Add(BuildNewEnroller(i));
            }

            var enrollees = new List<Enrollee>();
            Random rnd = new Random();
            foreach (var enroller in enrollers)            {
                
                int numberOfEnrollees = rnd.Next(0, 4);
                if (numberOfEnrollees > 0)
                {
                    for (int i = 0; i < numberOfEnrollees; i++)
                    {
                        Enrollee enrollee = BuildNewEnrollee(i);
                        enrollee.Enroller = enroller;
                        enroller.Enrollees.Add(enrollee);
                        enrollees.Add(enrollee);
                    }
                }
            }

            context.PersonSet.AddRange(mentors);
            context.PersonSet.AddRange(enrollers);
            context.PersonSet.AddRange(enrollees);
            context.SaveChanges();

            // Set up event instructions
            mineCraftInstructions = new EventInstructions { ID = Guid.NewGuid(), Instructions = "These are the Minecraft instructions, <a href=\"http://www.minecraft.com\">Minecraft</a>" };
            htmlInstructions = new EventInstructions { ID = Guid.NewGuid(), Instructions = "These are the HTML instructions, <a href=\"http://www.w3c.org\">HTML</a>" };
            context.EventInstructionsSet.Add(mineCraftInstructions);
            context.EventInstructionsSet.Add(htmlInstructions);
            context.SaveChanges();

            // Set up locations
            for (int i = 0; i < 5; i++)
            {
                locations.Add(BuildNewLocation(i));
            }

            

            // Set up events and distribute them amongst locations
            List<Event> events1 = new List<Event>();
            for (int i = 0; i < 20; i++)
            {
                Event e = BuildNewEvent(1, i, i%2 == 0 ? mineCraftInstructions.ID : htmlInstructions.ID);
                events1.Add(e);
                locations[0].Events.Add(e);

                // Signup enrollees for some of the events here
                if(i == 0)
                {
                    int j = 0;
                    while(e.Enrollments.Count < 5)
                    {   
                        if (j >= enrollers.Count) break;
                        var enroller = enrollers[j];
                        foreach(var enrollee in enroller.Enrollees)
                        {
                            Enrollment enrollment = new Enrollment
                            {
                                Enrollee = enrollee,
                                Enroller = enroller,
                                EnrollmentDate = DateTime.Now,
                                EnrollmentID = Guid.NewGuid(),
                                Event = e,
                                EventID = e.EventID
                            };
                            
                            if(e.Enrollments.Count < 5)
                            {
                                e.Enrollments.Add(enrollment);
                                enrollments.Add(enrollment);
                            }
                        }
                        j++;
                    }
                }

                // 10 enrollees here
                if (i == 1)
                {
                    int j = 0;
                    while (e.Enrollments.Count < 10)
                    {
                        if (j >= enrollers.Count) break;
                        var enroller = enrollers[j];
                        foreach (var enrollee in enroller.Enrollees)
                        {
                            Enrollment enrollment = new Enrollment
                            {
                                Enrollee = enrollee,
                                Enroller = enroller,
                                EnrollmentDate = DateTime.Now,
                                EnrollmentID = Guid.NewGuid(),
                                Event = e,
                                EventID = e.EventID
                            };

                            if (e.Enrollments.Count < 10)
                            {
                                e.Enrollments.Add(enrollment);
                                enrollments.Add(enrollment);
                            }
                        }

                        j++;
                    }
                }

                // 15 enrollees here
                if (i == 2)
                {
                    int j = 0;
                    while (e.Enrollments.Count < 15)
                    {
                        if (j >= enrollers.Count) break;
                        var enroller = enrollers[j];
                        foreach (var enrollee in enroller.Enrollees)
                        {
                            Enrollment enrollment = new Enrollment
                            {
                                Enrollee = enrollee,
                                Enroller = enroller,
                                EnrollmentDate = DateTime.Now,
                                EnrollmentID = Guid.NewGuid(),
                                Event = e,
                                EventID = e.EventID
                            };

                            if (e.Enrollments.Count < 15)
                            {
                                e.Enrollments.Add(enrollment);
                                enrollments.Add(enrollment);
                            }
                        }
                        j++;
                    }
                }

                // 16 enrollees here
                if (i == 3)
                {
                    int j = 0;
                    while (e.Enrollments.Count < 16)
                    {
                        if (j >= enrollers.Count) break;
                        var enroller = enrollers[j];
                        foreach (var enrollee in enroller.Enrollees)
                        {
                            Enrollment enrollment = new Enrollment
                            {
                                Enrollee = enrollee,
                                Enroller = enroller,
                                EnrollmentDate = DateTime.Now,
                                EnrollmentID = Guid.NewGuid(),
                                Event = e,
                                EventID = e.EventID
                            };

                            if (e.Enrollments.Count < 16)
                            {
                                e.Enrollments.Add(enrollment);
                                enrollments.Add(enrollment);
                            }
                        }
                        j++;
                    }
                }

                // 19 enrollees here
                if (i == 4)
                {
                    int j = 0;
                    while (e.Enrollments.Count < 19)
                    {
                        if (j >= enrollers.Count) break;
                        var enroller = enrollers[j];
                        foreach (var enrollee in enroller.Enrollees)
                        {
                            Enrollment enrollment = new Enrollment
                            {
                                Enrollee = enrollee,
                                Enroller = enroller,
                                EnrollmentDate = DateTime.Now,
                                EnrollmentID = Guid.NewGuid(),
                                Event = e,
                                EventID = e.EventID
                            };

                            if (e.Enrollments.Count < 19)
                            {
                                e.Enrollments.Add(enrollment);
                                enrollments.Add(enrollment);
                            }
                        }

                        j++;
                    }
                }

                // 20 enrollees here
                if (i == 5)
                {
                    int j = 0;
                    while (e.Enrollments.Count < 20)
                    {
                        if (j >= enrollers.Count) break;
                        var enroller = enrollers[j];
                        foreach (var enrollee in enroller.Enrollees)
                        {
                            Enrollment enrollment = new Enrollment
                            {
                                Enrollee = enrollee,
                                Enroller = enroller,
                                EnrollmentDate = DateTime.Now,
                                EnrollmentID = Guid.NewGuid(),
                                Event = e,
                                EventID = e.EventID
                            };

                            if (e.Enrollments.Count < 20)
                            {
                                e.Enrollments.Add(enrollment);
                                enrollments.Add(enrollment);
                            }
                        }

                        j++;
                    }
                }

                // 21 enrollees here
                if (i == 6)
                {
                    int j = 0;
                    while (e.Enrollments.Count < 21)
                    {
                        if (j >= enrollers.Count) break;
                        var enroller = enrollers[j];
                        foreach (var enrollee in enroller.Enrollees)
                        {
                            Enrollment enrollment = new Enrollment
                            {
                                Enrollee = enrollee,
                                Enroller = enroller,
                                EnrollmentDate = DateTime.Now,
                                EnrollmentID = Guid.NewGuid(),
                                Event = e,
                                EventID = e.EventID
                            };

                            if (e.Enrollments.Count < 21)
                            {
                                e.Enrollments.Add(enrollment);
                                enrollments.Add(enrollment);
                            }
                        }

                        j++;
                    }
                }
            }

            List<Event> events2 = new List<Event>();
            for (int i = 0; i < 20; i++)
            {
                Event e = BuildNewEvent(2, i, i % 2 == 0 ? mineCraftInstructions.ID : htmlInstructions.ID);
                events2.Add(e);
                locations[1].Events.Add(e);
            }

            List<Event> events3 = new List<Event>();
            for (int i = 0; i < 20; i++)
            {
                Event e = BuildNewEvent(3, i, i % 2 == 0 ? mineCraftInstructions.ID : htmlInstructions.ID);
                events3.Add(e);
                locations[2].Events.Add(e);
            }

            List<Event> events4 = new List<Event>();
            for (int i = 0; i < 20; i++)
            {
                Event e = BuildNewEvent(4, i, i % 2 == 0 ? mineCraftInstructions.ID : htmlInstructions.ID);
                events4.Add(e);
                locations[3].Events.Add(e);
            }

            List<Event> events5 = new List<Event>();
            for (int i = 0; i < 20; i++)
            {
                Event e = BuildNewEvent(5, i, i % 2 == 0 ? mineCraftInstructions.ID : htmlInstructions.ID);
                events5.Add(e);
                locations[4].Events.Add(e);
            }

            context.LocationSet.AddRange(locations);
            context.EventSet.AddRange(events1);
            context.EventSet.AddRange(events2);
            context.EventSet.AddRange(events3);
            context.EventSet.AddRange(events4);
            context.EventSet.AddRange(events5);
            context.EnrollmentSet.AddRange(enrollments);
            context.SaveChanges();
        }

        private Event BuildNewEvent(int eventIndex, int eventNumber, Guid instructionsID)
        {
            DateTime eventDate = DateTime.UtcNow.AddDays(30 + eventIndex + eventNumber);
            return new Event
            {
                CreatedBy = "ngrier",
                CreatedOn = DateTime.UtcNow,
                Enrollments = new List<Enrollment>(),
                EventDateTime = eventDate,
                EventID = Guid.NewGuid(),
                EventInstructionsID = instructionsID,
                EventName = "Awesome Sauce " + eventIndex + eventNumber,
                ImageName = instructionsID == mineCraftInstructions.ID ? "minecraftlogo.jpg" : "html-code.jpg",
                Locations = new List<Location>(),
                MaximumSignUps = 20,
                ModifiedBy = null,
                ModifiedOn = null,
                RegistrationEnd = eventDate.AddMinutes(-1),
                RegistrationStart = eventDate.AddMinutes(-30),
                SignUpEnd = eventDate.AddDays(-1),
                SignUpStart = DateTime.UtcNow.AddDays(-10)
            };
        }

        private Location BuildNewLocation(int i)
        {
            return new Location
            {
                Address1 = i + " Location Pass",
                Address2 = i + " Second Line",
                City = "Grand Central",
                Country = "CA",
                CreatedBy = "ngrier",
                CreatedOn = DateTime.UtcNow,
                Email = i + "location@tester" + i + ".loc",
                Events = new List<Event>(),
                Latitude = -35.321 + i,
                LocationID = Guid.NewGuid(),
                LocationName = "Coder Dojo Location" + i,
                Longitude = 55.3324 - i,
                ModifiedBy = null,
                ModifiedOn = null,
                PhoneDisplay = "1-800 484-7848",
                Phone = "18004847848",
                PhoneType = PhoneType.Office,
                PostalCode = "4874" + i,
                Region = "Kandahar",
                StateProvince = "QB",
                UnitNumber = "1" + i
            };
        }

        private Enrollee BuildNewEnrollee(int i)
        {
            return new Enrollee
            {
                Address1 = i + " Enrollee Blvd",
                Address2 = null,
                Country = "US",
                PostalCode = i + "9876",
                Region = null,
                StateProvince = "WI",
                UnitNumber = null,
                PhoneDisplay = "+44 91 933 0090",
                Phone = "44919330090",
                PhoneType = PhoneType.Mobile,
                Email = i + "enrollee@tester" + i + ".com",
                CreatedBy = "ngrier",
                CreatedOn = DateTime.UtcNow,
                FirstName = "Enrollee" + i,
                LastName = "Last" + i,
                ModifiedBy = null,
                ModifiedOn = null,
                PersonID = Guid.NewGuid()
            };            
        }

        private Enroller BuildNewEnroller(int i)
        {
            return new Enroller
            {
                Address1 = i + " Enroller Road",
                Address2 = null,
                Country = "US",
                PostalCode = i + "9876",
                Region = null,
                StateProvince = "WI",
                UnitNumber = null,
                PhoneDisplay = "+44 91 933 0090",
                Phone = "44919330090",
                PhoneType = PhoneType.Mobile,
                Email = i + "enroller@tester" + i + ".com",
                Enrollees = new List<Enrollee>(),
                CreatedBy = "ngrier",
                CreatedOn = DateTime.UtcNow,
                FirstName = "Enroller" + i,
                LastName = "Last" + i,
                ModifiedBy = null,
                ModifiedOn = null,
                PersonID = Guid.NewGuid()
            };
        }

        private Mentor BuildNewMentor(int i)
        {
            return new Mentor
            {
                Address1 = i + " Mentor Lane",
                Address2 = null,
                Country = "US",
                PostalCode = i + "9876",
                Region = null,
                StateProvince = "WI",
                UnitNumber = null,
                PhoneDisplay = "+44 91 933 0090",
                Phone = "44919330090",
                PhoneType = PhoneType.Mobile,
                Email = i + "mentor@tester" + i + ".com",
                CreatedBy = "ngrier",
                CreatedOn = DateTime.UtcNow,
                FirstName = "Mentor" + i,
                LastName = "Last" + i,
                ModifiedBy = null,
                ModifiedOn = null,
                PersonID = Guid.NewGuid()                
            };
        }
    }
}