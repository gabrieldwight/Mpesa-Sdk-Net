using FluentValidation;
using MpesaSdk.Dtos;

namespace MpesaSdk.Validators
{
    public class DynamicMpesaQRValidator : AbstractValidator<DynamicMpesaQR>
    {
        public DynamicMpesaQRValidator()
        {
            RuleFor(x => x.MerchantName)
                .NotNull()
                .WithMessage("{PropertyName} - Merchant name is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Merchant name should not be empty.");

            RuleFor(x => x.RefNo)
                .NotNull()
                .WithMessage("{PropertyName} - Transaction reference is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Transaction reference should not be empty.");

            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage("{PropertyName} - Transaction amount is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Transaction amount should not be empty.");

            RuleFor(x => x.TrxCode)
                .NotNull()
                .WithMessage("{PropertyName} - Transaction type is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Transaction type should not be empty.");

            RuleFor(x => x.CPI)
                .NotNull()
                .WithMessage("{PropertyName} - Credit Party Identifier is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Credit Party Identifier should not be empty.");
        }
    }
}
