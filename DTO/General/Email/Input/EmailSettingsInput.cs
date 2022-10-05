namespace DTO.General.Email.Input
{
    public class EmailSettingsInput
    {
        public EmailSettingsInput(string email, string displayName, string pass, string host, int port)
        {
            Email = email;
            DisplayName = displayName;
            Password = pass;
            Host = host;
            Port = port;
        }

        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
