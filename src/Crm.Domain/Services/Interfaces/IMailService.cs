using System.Threading.Tasks;
using Crm.Domain;

namespace Crm.Domain.Services.Interfaces {
    public interface IMailService {
        Task SendPasswordResetMail(User user);
        Task SendActivationEmail(User user);
        Task SendCreationEmail(User user);
    }
}
