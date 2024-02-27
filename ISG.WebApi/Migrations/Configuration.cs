namespace ISG.WebApi.Migrations
{
    using ISG.WebApi.Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ISG.WebApi.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        private DateTime GuncelTarih()
        {
            TimeZoneInfo customTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.Now, customTimeZone);
        }
        protected override void Seed(ISG.WebApi.Infrastructure.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "SuperPowerUser",
                Email = "taiseer.joudeh@gmail.com",
                EmailConfirmed = true,
                FirstName = "Taiseer",
                LastName = "Joudeh",
                Level = 1,
                JoinDate = GuncelTarih().AddYears(-3)
            };

            manager.Create(user, "MySuperP@ss!");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "ISG_Admin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "ISG_Hekim" });
                roleManager.Create(new IdentityRole { Name = "ISG_SaglikMemuru" });
                roleManager.Create(new IdentityRole { Name = "ISG_Denetci" });
                roleManager.Create(new IdentityRole { Name = "ISG_Memur" });
                roleManager.Create(new IdentityRole { Name = "ISG_Uzman" });
                roleManager.Create(new IdentityRole { Name = "IK_Memur" });
                roleManager.Create(new IdentityRole { Name = "IK_Mudur" });
                roleManager.Create(new IdentityRole { Name = "Bol_Amiri" });
                roleManager.Create(new IdentityRole { Name = "Bol_Memuru" });
            }

            var adminUser = manager.FindByName("SuperPowerUser");

            manager.AddToRoles(adminUser.Id, new string[] { "ISG_Admin", "Admin" });
        }
    }
}
