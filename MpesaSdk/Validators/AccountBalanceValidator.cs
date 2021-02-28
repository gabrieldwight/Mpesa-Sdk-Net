using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
    public class AccountBalanceValidator : AbstractValidator<AccountBalance>
    {
        public AccountBalanceValidator()
        {
            RuleFor(x => x.PartyA)
                .NotNull()
                .WithMessage("{PropertyName} - The organization paybill/till number shortcode is required.")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The organization paybill/till number shortcode should be in numeric value.")
                .Length(5, 7)
                .WithMessage("{PropertyName} - The organization paybill/till number shortcode should be 5 to 7 digits account number.");

            RuleFor(x => x.IdentifierType)
                .NotNull()
                .WithMessage("{PropertyName} - Organization identifier is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Organization identifier should not be empty.");

            RuleFor(x => x.Remarks)
                .NotNull()
                .WithMessage("{PropertyName} - Reason for account balance is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Reason for account balance should not be empty.");

            RuleFor(x => x.Initiator)
                .NotNull()
                .WithMessage("{PropertyName} - The name of the initiator is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - The name of the intiator should not be empty.");

            RuleFor(x => x.QueueTimeOutURL)
                .NotNull()
                .WithMessage("{PropertyName} - The queuetimeout url is required.")
                .Must(x => LinkMustBeAUri(x))
                .WithMessage("{PropertyName} - The queuetimeout url should be a valid secure url.");

            RuleFor(x => x.ResultURL)
                .NotNull()
                .WithMessage("{PropertyName} - The result url is required.")
                .Must(x => LinkMustBeAUri(x))
                .WithMessage("{PropertyName} - The result url should be a valid secure url.");
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
