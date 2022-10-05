using System.Collections.Generic;
using System.Net.Mail;

namespace DTO.General.Email.Input
{
    public class EmailRequestInput
    {
        public EmailRequestInput(string emailTo, string subject, string body)
        {
            EmailTo = emailTo;
            Subject = subject;
            Body = body;
        }

        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
