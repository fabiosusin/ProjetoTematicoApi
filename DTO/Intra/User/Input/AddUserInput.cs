using DTO.Intra.User.Enums;

namespace DTO.Intra.User.Input
{
    public class AddUserInput
    {
        public bool IsMasterAdmin { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordValidation { get; set; }
        public string Username { get; set; }
        public string TempPassword { get; set; }
        public RouteType Permissions { get; set; }
    }
}
