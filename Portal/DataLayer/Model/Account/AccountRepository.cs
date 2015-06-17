using System.Linq;
using DataLayer.Entitties;
using DataLayer.Repository.Shared;

namespace DataLayer.Model.Account
{
    public class AccountRepository : BaseRepository
    {

        public bool IsUserIdAndPasswordCorrect(int userId, string hashPassword)
        {
            return DbConn.AspNetUsers
                .Any(e => e.Id == userId && e.PasswordHash == hashPassword);
        }

        public int GetRoleIdByUsername(string username)
        {
            return DbConn.AspNetUsers.Where(e => e.UserName.Trim().ToLower() == username.Trim().ToLower())
                .Select(e => e.AspNetRoles.Select(i => i.Id).FirstOrDefault()).FirstOrDefault();
        }
        public string GetRoleDescByUsername(string userName)
        {
            return DbConn.AspNetUsers.Where(e => e.UserName == userName)
                .Select(e => e.AspNetRoles.Select(i => i.Description).FirstOrDefault()).FirstOrDefault();
        }

        public static int GetIdByUsername(string username, PuEntities dbConn)
        {
            return dbConn.AspNetUsers.Where(e => e.UserName.Trim().ToLower() == username.Trim().ToLower())
                .Select(e => e.Id).Single();
        }

        public static int GetIdByUsername(string username)
        {
            using (var db = new PuEntities())
            {
                return db.AspNetUsers.Where(e => e.UserName.Trim().ToLower() == username.Trim().ToLower())
                    .Select(e => e.Id).Single();
            }
        }

    }

}
