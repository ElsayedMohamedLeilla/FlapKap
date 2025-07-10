using FlapKap.Models.DTOs.Users;
using FluentValidation;

namespace FlapKap.Validation.FluentValidation.Users
{
    public class DepositModelValidator : AbstractValidator<DepositModel>
    {
        public DepositModelValidator()
        {
            RuleFor(x => x.DepositAmount)
                .Must(value => value > 0)
                .WithMessage("Deposit must greater than zero");

            RuleFor(x => x.DepositAmount)
                .Must(n => new List<decimal> { 5, 10, 20, 50, 100 }.Contains(n))
                .WithMessage("Deposit must be one of the allowed values: 5, 10, 20, 50, or 100.");
        }
    }
}
