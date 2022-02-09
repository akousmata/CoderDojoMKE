namespace CoderDojoMKE.Web.Migrations
{
    using Models.Auth;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;    
    using Microsoft.AspNet.Identity;
    using Models.Data;
    using CoderDojoMKE.Models;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //RoleStore<ApplicationRole> roleStore = new RoleStore<ApplicationRole>(context);
            //RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(roleStore);

            //ApplicationRole role = new ApplicationRole("GlobalAdmin", "Global Access");
            //IdentityResult idResult = roleManager.Create(role);

            //ApplicationRole mentorRole = new ApplicationRole("Mentor", "Access to enrollment resolutions, enrollments, reports");
            //IdentityResult mentorResult = roleManager.Create(mentorRole);

            //ApplicationRole standardRole = new ApplicationRole("StandardUser", "Access to enroll an enrollee");
            //IdentityResult standardResult = roleManager.Create(standardRole);

            //UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //ApplicationUser globalAdmin = new ApplicationUser { UserName = "nathan.grier@gmail.com", Email = "nathan.grier@gmail.com" };
            //IdentityResult result = userManager.Create(globalAdmin, "diapente5");
            //userManager.AddToRole(globalAdmin.Id, "GlobalAdmin");
            //userManager.AddToRole(globalAdmin.Id, "Mentor");
            //context.EnrollerSet.AddOrUpdate(
            //    new Enroller
            //    {
            //        Address1 = "5532A W Wells Street",
            //        Address2 = null,
            //        Country = "US",
            //        PostalCode = "53208",
            //        Region = null,
            //        StateProvince = "WI",
            //        UnitNumber = null,
            //        PhoneDisplay = "(414) 897 3317",
            //        Phone = "4148973317",
            //        PhoneType = PhoneType.Mobile,
            //        Email = "nathan.grier@gmail.com",
            //        Enrollees = new List<Enrollee>(),
            //        CreatedBy = "ngrier",
            //        CreatedOn = DateTime.UtcNow,
            //        FirstName = "Nathan",
            //        LastName = "Grier",
            //        ModifiedBy = null,
            //        ModifiedOn = null,
            //        PersonID = Guid.Parse(globalAdmin.Id)
            //    });
        }
    }
}
