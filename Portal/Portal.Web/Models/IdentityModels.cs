using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Portal.Web.Models
{
    public class ApplicationUserLogin : IdentityUserLogin<int> { }
    public class ApplicationUserClaim : IdentityUserClaim<int> { }
    public class ApplicationUserRole : IdentityUserRole<int> { }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsObsolete { get; set; }

    }

    public static class IdentityExtensions2
    {
        /// <summary>
        ///     Return the full name using the UserContext
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string GetFullName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var id = identity.GetUserId<int>();
            var user = userManager.Users.FirstOrDefault(u => u.Id == id);

            return user == null ? null : string.Format("{0} {1}", user.FirstName, user.LastName);
        }
    }

    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>
    {
        public ApplicationRole() { }

        public ApplicationRole(string name)
            : this()
        {
            Name = name;
        }

        public ApplicationRole(string name, string description)
            : this(name)
        {
            Description = description;
        }

        public string Description { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("EntityConnection")
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUserStore()
            : this(new IdentityDbContext())
        {
            DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }

    public class ApplicationRoleStore : RoleStore<ApplicationRole, int, ApplicationUserRole>
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context)
            : base(context)
        {

        }
    }

    public class IdentityManager
    {
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<ApplicationRole, int>(
                new ApplicationRoleStore(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }

        public bool CreateRole(string name)
        {
            var rm = new RoleManager<ApplicationRole, int>(
                new ApplicationRoleStore(new ApplicationDbContext()));
            var idResult = rm.Create(new ApplicationRole(name));

            rm.FindByName(name).Description = "-";
            return idResult.Succeeded;
        }

        public bool CreateUser(ApplicationUser user, string password)
        {
            var um = new UserManager<ApplicationUser, int>(
                new ApplicationUserStore(new ApplicationDbContext()));
            var idResult = um.Create(user, password);
            return idResult.Succeeded;
        }

        public bool AddUserToRole(int userId, string roleName)
        {
            var um = new UserManager<ApplicationUser, int>(
                new ApplicationUserStore(new ApplicationDbContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }

        public void ClearUserRoles(int userId)
        {
            var um = new UserManager<ApplicationUser, int>(
                new ApplicationUserStore(new ApplicationDbContext()));
            var user = um.FindById(userId);
            var currentRoles = new List<ApplicationUserRole>();
            currentRoles.AddRange(user.Roles);

            var rm = new RoleManager<ApplicationRole, int>(
                new ApplicationRoleStore(new ApplicationDbContext()));

            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, rm.FindById(role.RoleId).Name);
            }
        }
    }
}