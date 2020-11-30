using Crm.Crosscutting.Constants;

namespace Crm.Crosscutting.Exceptions {
    public class InternalServerErrorException : BaseException {
        public InternalServerErrorException(string message) : base(ErrorConstants.DefaultType, message)
        {
        }
    }
}
