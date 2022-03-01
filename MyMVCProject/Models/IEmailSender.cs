using System.Threading.Tasks;

namespace MyMVCProject.Models
{
    public interface IEmailSender
    {
        Task SendAsync(string from, string to, string subject, string body);
    }
}