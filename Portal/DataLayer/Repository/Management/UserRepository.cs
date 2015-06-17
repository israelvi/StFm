using System.Linq;
using DataLayer.Model.Management;
using DataLayer.Repository.Shared;

namespace DataLayer.Repository.Management
{
    public class UserRepository : BaseRepository
    {
        public UserModel FindModelById(int id)
        {
            return DbConn.AspNetUsers.Where(e => e.Id == id).Select(e => new UserModel
            {
                Id = e.Id,
                Email = e.Email,
                LastName = e.LastName,
                FirstName = e.FirstName
            }).FirstOrDefault();
        }
    }

}
