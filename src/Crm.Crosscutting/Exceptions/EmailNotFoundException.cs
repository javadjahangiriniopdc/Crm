using Crm.Crosscutting.Constants;

namespace Crm.Crosscutting.Exceptions {
    public class EmailNotFoundException : BaseException {
        public EmailNotFoundException() : base(ErrorConstants.EmailNotFoundType, "Email address not registered")
        {
        }
    }
}
