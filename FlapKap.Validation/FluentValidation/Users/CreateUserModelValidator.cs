using FlapKap.Models.DTOs.Users;
using FluentValidation;

namespace FlapKap.Validation.FluentValidation.Users
{
    public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserModelValidator()
        {
            RuleFor(model => model.UserName).NotNull().
                   WithMessage("Enter User Name");

            RuleFor(x => x.UserName)
             .Matches("^[a-zA-Z0-9]+$")
             .WithMessage("User Name must contain only letters or digits.");

            RuleFor(model => model.Password).NotNull().
                    WithMessage("Enter Password");

            RuleFor(model => model.Role)
                .IsInEnum().
                WithMessage("Enter correct Role");
        }
    }
}
