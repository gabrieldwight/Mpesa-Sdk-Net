using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
    public class CustomerToBusinessRegisterUrlValidator : AbstractValidator<CustomerToBusinessRegisterUrl>
    {
        public CustomerToBusinessRegisterUrlValidator()
        {
            RuleFor(x => x.ShortCode)
                .NotNull()
                .WithMessage("{PropertyName} - The organization paybill or till number shortcode is required.")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The organization paybill or till number must be a numeric value.")
                .Length(5, 7)
                .WithMessage("{PropertyName} - The organization paybill or till number should be 5 to 7 digits account number.");

            RuleFor(x => x.ResponseType)
                .NotNull()
                .WithMessage("{PropertyName} - The response type for validation URL is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - The response type for validation URL should not be empty.");

            RuleFor(x => x.ConfirmationURL)
                .NotNull()
                .WithMessage("{PropertyName} - The confirmation url is required.")
                .Must(x => LinkMustBeAUri(x))
                .WithMessage("{PropertyName} - The confirmation url should be a valid secure url.");

            RuleFor(x => x.ValidationURL)
                .NotNull()
                .WithMessage("{PropertyName} - The validation url is required.")
                .Must(x => LinkMustBeAUri(x))
                .WithMessage("{PropertyName} - The validation url should be a valid secure url.");
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
