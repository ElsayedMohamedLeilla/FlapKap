using FlapKap.Models.DTOs.UserActions;
using FluentValidation;

namespace FlapKap.Validation.FluentValidation.UserActions
{
    public class BuyModelValidator : AbstractValidator<BuyModel>
    {
        public BuyModelValidator()
        {
            RuleFor(x => x.Items)
                .Must(value => value != null && value.Count > 0)
                .WithMessage("You Must at least one item to buy");

            RuleForEach(x => x.Items).SetValidator(new BuyItemModelValidator());

            RuleFor(x => x.Items)
             .Must(items => items.Select(t => t.ProductId).Distinct().Count() == items.Count)
             .WithMessage("Duplicate items are not allowed.");

        }
    }
    public class BuyItemModelValidator : AbstractValidator<BuyItemModel>
    {
        public BuyItemModelValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId is required");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}
