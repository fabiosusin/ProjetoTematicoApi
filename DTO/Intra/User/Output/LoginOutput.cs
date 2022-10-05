using DTO.General.Base.Api.Output;
using DTO.Intra.User.Database;
using System;

namespace DTO.Intra.User.Output
{
    public class LoginOutput : BaseApiOutput
    {
        public LoginOutput(string msg) : base(msg) { }

        public LoginOutput(Database.User input) : base(true)
        {
            if (input == null)
                return;

            Id = input.Id;  
            Name = input.Name;
            Email = input.Email;
            Password = input.Password;
            TempPassword = input.TempPassword;
            IsMasterUser = input.IsMasterAdmin;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TempPassword { get; set; }
        public string AccessToken { get; set; }
        public bool IsMasterUser { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}
