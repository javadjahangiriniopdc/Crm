using System.Security.Authentication;

namespace Crm.Crosscutting.Exceptions {
    public class UserNotActivatedException : AuthenticationException {
        public UserNotActivatedException(string message) : base(message)
        {
        }
    }
}
