using DTO.General.Email.Input;
using System.Threading.Tasks;

namespace DTO.Interface
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailRequestInput input);
    }
}
