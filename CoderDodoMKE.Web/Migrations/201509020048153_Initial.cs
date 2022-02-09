namespace CoderDojoMKE.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        PersonID = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        PhoneDisplay = c.String(),
                        PhoneType = c.Int(nullable: false),
                        PostalCode = c.String(),
                        Region = c.String(),
                        StateProvince = c.String(),
                        UnitNumber = c.String(),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.PersonID);
            
            CreateTable(
                "dbo.EnrollmentResolution",
                c => new
                    {
                        EnrollmentResolutionID = c.Guid(nullable: false),
                        EnrollmentID = c.Guid(nullable: false),
                        ResolutionDate = c.DateTime(nullable: false),
                        ResolutionResult = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EnrollmentResolutionID)
                .ForeignKey("dbo.Enrollment", t => t.EnrollmentResolutionID)
                .Index(t => t.EnrollmentResolutionID);
            
            CreateTable(
                "dbo.Enrollment",
                c => new
                    {
                        EnrollmentID = c.Guid(nullable: false),
                        EventID = c.Guid(nullable: false),
                        EnrollmentDate = c.DateTime(nullable: false),
                        Enrollee_PersonID = c.Guid(nullable: false),
                        Enroller_PersonID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.EnrollmentID)
                .ForeignKey("dbo.Enrollee", t => t.Enrollee_PersonID)
                .ForeignKey("dbo.Enroller", t => t.Enroller_PersonID)
                .ForeignKey("dbo.Event", t => t.EventID, cascadeDelete: true)
                .Index(t => t.EventID)
                .Index(t => t.Enrollee_PersonID)
                .Index(t => t.Enroller_PersonID);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        EventID = c.Guid(nullable: false),
                        EventName = c.String(),
                        MaximumSignUps = c.Int(nullable: false),
                        ImageName = c.String(),
                        EventDateTime = c.DateTime(nullable: false),
                        SignUpStart = c.DateTime(nullable: false),
                        EventInstructionsID = c.Guid(nullable: false),
                        SignUpEnd = c.DateTime(nullable: false),
                        RegistrationStart = c.DateTime(nullable: false),
                        RegistrationEnd = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.EventID)
                .ForeignKey("dbo.EventInstructions", t => t.EventInstructionsID, cascadeDelete: true)
                .Index(t => t.EventInstructionsID);
            
            CreateTable(
                "dbo.EventInstructions",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Instructions = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        LocationID = c.Guid(nullable: false),
                        LocationName = c.String(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        PhoneDisplay = c.String(),
                        PhoneType = c.Int(nullable: false),
                        PostalCode = c.String(),
                        Region = c.String(),
                        StateProvince = c.String(),
                        UnitNumber = c.String(),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedBy = c.String(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.LocationID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Role_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.Role_Id)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.UserId)
                .Index(t => t.RoleId)
                .Index(t => t.Role_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EventLocation",
                c => new
                    {
                        Event_EventID = c.Guid(nullable: false),
                        Location_LocationID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_EventID, t.Location_LocationID })
                .ForeignKey("dbo.Event", t => t.Event_EventID, cascadeDelete: true)
                .ForeignKey("dbo.Location", t => t.Location_LocationID, cascadeDelete: true)
                .Index(t => t.Event_EventID)
                .Index(t => t.Location_LocationID);
            
            CreateTable(
                "dbo.Enrollee",
                c => new
                    {
                        PersonID = c.Guid(nullable: false),
                        Enroller_PersonID = c.Guid(nullable: false),
                        EnrollerID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("dbo.Person", t => t.PersonID)
                .ForeignKey("dbo.Enroller", t => t.Enroller_PersonID)
                .Index(t => t.PersonID)
                .Index(t => t.Enroller_PersonID);
            
            CreateTable(
                "dbo.Enroller",
                c => new
                    {
                        PersonID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("dbo.Person", t => t.PersonID)
                .Index(t => t.PersonID);
            
            CreateTable(
                "dbo.Mentor",
                c => new
                    {
                        PersonID = c.Guid(nullable: false),
                        LastBackgroundCheck = c.DateTime(),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("dbo.Person", t => t.PersonID)
                .Index(t => t.PersonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Mentor", "PersonID", "dbo.Person");
            DropForeignKey("dbo.Enroller", "PersonID", "dbo.Person");
            DropForeignKey("dbo.Enrollee", "Enroller_PersonID", "dbo.Enroller");
            DropForeignKey("dbo.Enrollee", "PersonID", "dbo.Person");
            DropForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.EnrollmentResolution", "EnrollmentResolutionID", "dbo.Enrollment");
            DropForeignKey("dbo.EventLocation", "Location_LocationID", "dbo.Location");
            DropForeignKey("dbo.EventLocation", "Event_EventID", "dbo.Event");
            DropForeignKey("dbo.Event", "EventInstructionsID", "dbo.EventInstructions");
            DropForeignKey("dbo.Enrollment", "EventID", "dbo.Event");
            DropForeignKey("dbo.Enrollment", "Enroller_PersonID", "dbo.Enroller");
            DropForeignKey("dbo.Enrollment", "Enrollee_PersonID", "dbo.Enrollee");
            DropIndex("dbo.Mentor", new[] { "PersonID" });
            DropIndex("dbo.Enroller", new[] { "PersonID" });
            DropIndex("dbo.Enrollee", new[] { "Enroller_PersonID" });
            DropIndex("dbo.Enrollee", new[] { "PersonID" });
            DropIndex("dbo.EventLocation", new[] { "Location_LocationID" });
            DropIndex("dbo.EventLocation", new[] { "Event_EventID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "Role_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Event", new[] { "EventInstructionsID" });
            DropIndex("dbo.Enrollment", new[] { "Enroller_PersonID" });
            DropIndex("dbo.Enrollment", new[] { "Enrollee_PersonID" });
            DropIndex("dbo.Enrollment", new[] { "EventID" });
            DropIndex("dbo.EnrollmentResolution", new[] { "EnrollmentResolutionID" });
            DropTable("dbo.Mentor");
            DropTable("dbo.Enroller");
            DropTable("dbo.Enrollee");
            DropTable("dbo.EventLocation");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Location");
            DropTable("dbo.EventInstructions");
            DropTable("dbo.Event");
            DropTable("dbo.Enrollment");
            DropTable("dbo.EnrollmentResolution");
            DropTable("dbo.Person");
        }
    }
}
