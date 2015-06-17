using System;
using System.Data.Entity.Migrations;
using DataLayer.Model.Constants;
using Portal.Web.Models;

namespace Portal.Web.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            try
            {
                var idManager = new IdentityManager();
                bool success = idManager.CreateRole(RoleConstants.MANAGER);
                if (!success) return;

                var newUser = new ApplicationUser
                {
                    UserName = "Administrador",
                    FirstName = "Administrador",
                    LastName = "Scitum",
                    Email = "admin@scitum.com.mx"
                };

                // Be careful here - you  will need to use a password which will 
                // be valid under the password rules for the application, 
                // or the process will abort:
                success = idManager.CreateUser(newUser, "Admin123..");
                if (!success) return;

                success = idManager.AddUserToRole(newUser.Id, RoleConstants.MANAGER);
                if (!success)
                    base.Seed(context);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
