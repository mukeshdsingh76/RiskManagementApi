using System.Threading.Tasks;

namespace RM.Auth.Services
{
  public interface IEmailSender
  {
    Task SendEmailAsync(string email, string subject, string message);
  }
}
