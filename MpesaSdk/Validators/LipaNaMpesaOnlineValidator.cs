using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
    public class LipaNaMpesaOnlineValidator : AbstractValidator<LipaNaMpesaOnline>
    {
        public LipaNaMpesaOnlineValidator()
        {
            RuleFor(x => x.BusinessShortCode)
                .NotNull()
                .WithMessage("{PropertyName} - The paybill or till number shortcode should not be empty.")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The paybill or till number must be a numeric value.")
                .Length(5, 7)
                .WithMessage("{PropertyName} - The paybill or till number should be 5 to 7 account number digits.");

            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage("{PropertyName} - Amount is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Amount must not be empty")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The amount should be in numeric value.");

            RuleFor(x => x.PartyA)
                .NotNull()
                .WithMessage("{PropertyName} - The mobile number is required.")
                .SetValidator(new PhoneNumberValidator())
                .WithMessage("{PropertyName} - The mobile number should start with 2547XXXX.")
                .MaximumLength(12)
                .WithMessage("{PropertyName} - The mobile number should be 12 digit.");

            RuleFor(x => x.PartyB)
                .NotNull()
                .WithMessage("{PropertyName} - The paybill or till number is required.")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The paybill or till number must be a numeric value.")
                .Length(5, 7)
                .WithMessage("{PropertyName} - The paybill or till number should be 5 to 7 account number.");

            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .WithMessage("{PropertyName} - The mobile number is required.")
                .SetValidator(new PhoneNumberValidator())
                .WithMessage("{PropertyName} - The mobile number should start with 2547XXXX.")
                .MaximumLength(12)
                .WithMessage("{PropertyName} - The mobile number should be 12 digit.");

            RuleFor(x => x.CallBackURL)
                .NotNull()
                .WithMessage("{PropertyName} - The callback url is required.")
                .Must(x => LinkMustBeAUri(x))
                .WithMessage("{PropertyName} - The callback url should be a valid secure url.");

            RuleFor(x => x.AccountReference)
                .NotNull()
                .WithMessage("{PropertyName} - The account reference should not be empty.")
                .MaximumLength(12)
                .WithMessage("{PropertyName} - The account reference should not be more than 12 characters.");

            RuleFor(x => x.TransactionDesc)
                .NotNull()
                .WithMessage("{PropertyName} - The transaction description should not be empty.")
                .MaximumLength(13)
                .WithMessage("{PropertyName} - The transaction description should not be more than 13 characters.");
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
