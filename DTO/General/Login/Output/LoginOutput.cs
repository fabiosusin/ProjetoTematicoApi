using DTO.General.Base.Api.Output;
using System;
using DTO.Intra.User.Output;

namespace DTO.General.Login.Output
{
    public class LoginOutput : BaseApiOutput
    {
        public LoginOutput(string msg) : base(msg) { }

        public LoginOutput(Intra.User.Output.LoginOutput input) : base(true)
        {
            if (input == null)
                return;

            Id = input.Id;
            Email = input.Email;
            AccessToken = input.AccessToken;
            AccessTokenExpiration = input.AccessTokenExpiration;
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string Cellphone { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}
