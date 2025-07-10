using FlapKap.Models.DTOs.Users;
using FluentValidation;

namespace FlapKap.Validation.FluentValidation.Users
{
    public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
    {
        public UpdateUserModelValidator()
        {
            RuleFor(model => model.Id).NotNull().
                  WithMessage("You Must Enter User Id");

            RuleFor(model => model.UserName).NotNull().
                   WithMessage("Enter User Name");

            RuleFor(x => x.UserName)
             .Matches("^[a-zA-Z0-9]+$")
             .WithMessage("User Name must contain only letters or digits.");

            RuleFor(model => model.Role)
                .IsInEnum().
                WithMessage("Enter correct Role");
        }
    }
}
