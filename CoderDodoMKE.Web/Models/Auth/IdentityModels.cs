using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;
using CoderDojoMKE.Web.Models.Data;

namespace CoderDojoMKE.Web.Models.Auth
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name, string description)
            : base(name)
        {
            this.Description = description;
        }

        public string Description { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole
    {
        public ApplicationUserRole() : base() { }

        public ApplicationRole Role { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=CDMDatabase", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Enrollee> EnrolleeSet { get; set; }
        public DbSet<Enroller> EnrollerSet { get; set; }
        public DbSet<Enrollment> EnrollmentSet { get; set; }
        public DbSet<EnrollmentResolution> EnrollmentResolutionSet { get; set; }
        public DbSet<EventInstructions> EventInstructionsSet { get; set; }
        public DbSet<Event> EventSet { get; set; }
        public DbSet<Location> LocationSet { get; set; }
        public DbSet<Mentor> MentorSet { get; set; }
        public DbSet<Person> PersonSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Initialize using Table-Per-Type inheritance            
            modelBuilder.Entity<Enrollee>().ToTable("Enrollee").HasRequired(e => e.Enroller).WithMany(e => e.Enrollees);
            modelBuilder.Entity<Enroller>().ToTable("Enroller").HasMany(e => e.Enrollees).WithRequired(e => e.Enroller);
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment").HasRequired(e => e.Enrollee);
            modelBuilder.Entity<Enrollment>().HasRequired(e => e.Enroller);
            modelBuilder.Entity<EnrollmentResolution>().ToTable("EnrollmentResolution").HasRequired(e => e.Enrollment);
            modelBuilder.Entity<EventInstructions>().ToTable("EventInstructions");
            modelBuilder.Entity<Event>().ToTable("Event").HasMany(e => e.Enrollments).WithRequired(e => e.Event);
            modelBuilder.Entity<Event>().HasMany(e => e.Locations).WithMany(l => l.Events);
            modelBuilder.Entity<Event>().HasRequired(e => e.EventInstructions);
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Mentor>().ToTable("Mentor");
            modelBuilder.Entity<Person>().ToTable("Person");
            base.OnModelCreating(modelBuilder);
        }    
    }
}