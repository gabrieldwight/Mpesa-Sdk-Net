using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
    public class B2BExpressCheckoutValidator : AbstractValidator<B2BExpressCheckoutRequest>
    {
        public B2BExpressCheckoutValidator() 
        {
            RuleFor(x => x.PrimaryShortCode)
                .NotNull()
                .WithMessage("{PropertyName} - The paybill or till number shortcode should not be empty.")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The paybill or till number must be a numeric value.")
                .Length(5, 7)
                .WithMessage("{PropertyName} - The paybill or till number should be 5 to 7 account number digits.");

            RuleFor(x => x.ReceiverShortCode)
                .NotNull()
                .WithMessage("{PropertyName} - The receiver paybill or till number shortcode should not be empty.")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The receiver paybill or till number must be a numeric value.")
                .Length(5, 7)
                .WithMessage("{PropertyName} - The receiver paybill or till number should be 5 to 7 account number digits.");

            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage("{PropertyName} - Amount is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Amount must not be empty")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The amount should be in numeric value.");

            RuleFor(x => x.PaymentRef)
                .NotNull()
                .WithMessage("{PropertyName} - The payment reference should not be empty.")
                .MaximumLength(12)
                .WithMessage("{PropertyName} - The payment reference should not be more than 12 characters.");

            RuleFor(x => x.CallbackUrl)
                .NotNull()
                .WithMessage("{PropertyName} - The callback url is required.")
                .Must(x => LinkMustBeAUri(x))
                .WithMessage("{PropertyName} - The callback url should be a valid secure url.");

            RuleFor(x => x.PartnerName)
               .NotNull()
               .WithMessage("{PropertyName} - The partner name is required.")
               .NotEmpty()
               .WithMessage("{PropertyName} - This partner name should not be empty.");

            RuleFor(x => x.RequestRefId)
               .NotNull()
               .WithMessage("{PropertyName} - The unique identifier is required.")
               .NotEmpty()
               .WithMessage("{PropertyName} - The unique identifier should not be empty.");
        }

        private static bool LinkMustBeAUri(string link)
        {
            if (!Uri.IsWellFormedUriString(link, UriKind.Absolute))
            {
                return false;
            }
            return true;
        }
    }
}
