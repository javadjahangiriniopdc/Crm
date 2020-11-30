using System.Security.Authentication;

namespace Crm.Crosscutting.Exceptions {
    public class UsernameNotFoundException : AuthenticationException {
        public UsernameNotFoundException(string message) : base(message)
        {
        }
    }
}
