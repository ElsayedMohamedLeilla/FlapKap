using FlapKap.Models.Requests;
using FluentValidation;

namespace FlapKap.Validation.FluentValidation.Users
{
    public class SignInModelValidator : AbstractValidator<SignInModel>
    {
        public SignInModelValidator()
        {
            RuleFor(signInModel => signInModel.UserName).NotNull().
                    WithMessage("Enter User Name");

            RuleFor(signInModel => signInModel.Password).NotNull().
                    WithMessage("Enter Password");
        }
    }
}
