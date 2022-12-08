using DTO.General.Base.Database;
using DTO.Intra.User.Input;

namespace DTO.Intra.User.Database
{
    public class AppUser : BaseData
    {
        public AppUser() { }
        public AppUser(AddUserInput input)
        {
            if (input == null)
                return;

            Name = input.Name;
            Id = input.UserId;
            Email = input.Email;
            Password = input.Password;
            Username = input.Username;
            IsMasterAdmin = input.IsMasterAdmin;
            TempPassword = input.TempPassword;
        }

        public bool IsMasterAdmin { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string TempPassword { get; set; }
    }

}
