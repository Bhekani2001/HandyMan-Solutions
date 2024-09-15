using System.Threading.Tasks;

namespace HandyMan_Solutions.Services
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string body);
    }
}
