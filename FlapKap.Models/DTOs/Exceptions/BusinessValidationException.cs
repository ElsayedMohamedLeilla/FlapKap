using FluentValidation.Results;

namespace FlapKap.Models.DTOs.Exceptions
{
    public class BusinessValidationException : Exception
    {
        public string MessageCode;
        public new string Message;

        public BusinessValidationException(string messageCode)
        {
            Message = messageCode;
        }
        public BusinessValidationException(string messageCode, string message) : base(message)
        {
            MessageCode = messageCode;
            Message = message;
        }
        public BusinessValidationException(ValidationResult validateResult)
        {
            var error = validateResult.Errors.FirstOrDefault();
            MessageCode = error.ErrorCode;
            Message = error.ErrorMessage;
        }
    }
}
