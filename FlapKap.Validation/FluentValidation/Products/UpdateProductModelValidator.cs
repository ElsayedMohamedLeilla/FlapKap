using FlapKap.Models.DTOs.Users;
using FluentValidation;

namespace FlapKap.Validation.FluentValidation.Users
{
    public class UpdateProductModelValidator : AbstractValidator<UpdateProductModel>
    {
        public UpdateProductModelValidator()
        {
            RuleFor(model => model.Id).NotNull().
                  WithMessage("You Must Enter User Id");

            RuleFor(model => model.Cost).GreaterThan(0).
                   WithMessage("Cost Must Greater Than Zero !");
            RuleFor(model => model.Name).NotNull().
                   WithMessage("You Must Enter Product Name");
            RuleFor(model => model.Quantity).GreaterThan(0).
                    WithMessage("Quantity Must Greater Than Zero !");
        }
    }
}
