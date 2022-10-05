namespace DTO.Intra.User.Input
{
    public class UpdateUserInput
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string NewPassword { get; set; }
        public string PasswordValidation { get; set; }
    }
}
