using DTO.General.Email.Input;
using DTO.Interface;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Services.Mobile.Email
{
    public class EmailService : IEmailService
    {

        private static readonly EmailSettingsInput _mailSettings = new("noreplyfabiosusin@gmail.com", "noreplyfabiosusin", "fabio210800", "smtp.gmail.com", 587);

        public Task<bool> SendEmail(EmailRequestInput input)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var email = new MailMessage
                    {
                        From = new MailAddress(_mailSettings.Email, _mailSettings.DisplayName),
                        Subject = input.Subject,
                        IsBodyHtml = false,
                        Body = input.Body
                    };

                    email.To.Add(new MailAddress(input.EmailTo));
                    if(input.Attachments!= null)
                    foreach (var file in input.Attachments)
                        email.Attachments.Add(file);

                    var smtp = new SmtpClient
                    {
                        Port = _mailSettings.Port,
                        Host = _mailSettings.Host,
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    await smtp.SendMailAsync(email);

                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}
