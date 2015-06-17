using System.ComponentModel.DataAnnotations;

namespace DataLayer.Model.Management
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        public string LastName { get; set; }

        //public Workshop ToEntity()
        //{
        //    return new Workshop
        //    {
        //        Id = Id,
        //        Name = Name,
        //        UserId = UserId,
        //    };
        //}

        //public Workshop UpdateModel(Workshop oldModel)
        //{
        //    oldModel.Id = Id;
        //    oldModel.Name = Name;
        //    oldModel.UserId = UserId;
        //    return oldModel;
        //}
    }
}
