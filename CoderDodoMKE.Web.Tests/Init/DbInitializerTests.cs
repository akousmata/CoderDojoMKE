using CoderDojoMKE.Web.Init;
using CoderDojoMKE.Web.Models.Auth;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace CoderDojoMKE.Web.Tests
{
    [TestClass]
    public class DbInitializerTests
    {
        [TestMethod]
        public void Can_Connect_To_DB()
        {             
            DbContext context = new ApplicationDbContext();
            Assert.IsTrue(context.Database.Exists());
        }

        [TestMethod]
        public void Can_Initialize_DB()
        {
            Database.SetInitializer(new CDMInitializer());
            IdentityDbContext<ApplicationUser> db = new ApplicationDbContext();
            db.Database.Initialize(force: true);
        }
    }
}
