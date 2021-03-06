using System.Threading.Tasks;
using Crm.Domain;
using Crm.Domain.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Crm.Domain.Services {
    public class MailService : IMailService {
        public MailService()
        {
        }

        // private readonly JHipsterSettings _jhipsterSettings;
        
        // public MailService(IOptions<JHipsterSettings> jhipsterSettings)
        // {
        //     _jhipsterSettings = jhipsterSettings.Value;
        // }

        public virtual Task SendPasswordResetMail(User user)
        {
            //TODO send reset Email
            return Task.FromResult(Task.CompletedTask);
        }

        public virtual Task SendActivationEmail(User user)
        {
            //TODO Activation Email
            return Task.FromResult(Task.CompletedTask);
        }

        public virtual Task SendCreationEmail(User user)
        {
            //TODO Creation Email
            return Task.FromResult(Task.CompletedTask);
        }
    }
}
