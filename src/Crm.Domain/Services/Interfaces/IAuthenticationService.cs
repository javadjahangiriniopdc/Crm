using System.Security.Principal;
using System.Threading.Tasks;

namespace Crm.Domain.Services.Interfaces {
    public interface IAuthenticationService {
        Task<IPrincipal> Authenticate(string username, string password);
    }
}
