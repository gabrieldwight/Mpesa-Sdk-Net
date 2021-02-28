using FluentValidation;
using MpesaSdk.Dtos;

namespace MpesaSdk.Validators
{
    public class LipaNaMpesaQueryValidator : AbstractValidator<LipaNaMpesaQuery>
    {
        public LipaNaMpesaQueryValidator()
        {
            RuleFor(x => x.BusinessShortCode)
                .NotNull()
                .WithMessage("{PropertyName} - The paybill or till number should not be empty.")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The paybill or till number must be a numeric value.")
                .Length(5, 7)
                .WithMessage("{PropertyName} - The paybill or till number should be 5 to 7 account number.");


            RuleFor(x => x.CheckoutRequestID)
                .NotNull()
                .WithMessage("{PropertyName} - The CheckoutRequestID is required")
                .NotEmpty()
                .WithMessage("{PropertyName} - The CheckoutRequestID must not be empty");
        }
    }
}
