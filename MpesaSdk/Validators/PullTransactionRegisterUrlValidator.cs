using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
    public class PullTransactionRegisterUrlValidator : AbstractValidator<PullTransactionRegisterUrl>
    {
        public PullTransactionRegisterUrlValidator()
        {
            RuleFor(x => x.ShortCode)
                .NotNull()
                .WithMessage("{PropertyName} - The organization paybill or till number shortcode is required.")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The organization paybill or till number must be a numeric value.")
                .Length(5, 7)
                .WithMessage("{PropertyName} - The organization paybill or till number should be 5 to 7 digits account number.");

            RuleFor(x => x.NominatedNumber)
                .NotNull()
                .WithMessage("{PropertyName} - The mobile number is required.")
                .SetValidator(new PhoneNumberValidator<PullTransactionRegisterUrl, string>())
                .WithMessage("{PropertyName} - The mobile number should start with 2547XXXX.")
                .MaximumLength(12)
                .WithMessage("{PropertyName} - The mobile number should be 12 digit.");

            RuleFor(x => x.CallBackURL)
                .NotNull()
                .WithMessage("{PropertyName} - The callback url is required.")
                .Must(x => LinkMustBeAUri(x))
                .WithMessage("{PropertyName} - The callback url should be a valid secure url.");
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
