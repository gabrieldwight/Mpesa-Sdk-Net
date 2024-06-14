using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
    public class B2CAccountTopUpValidator : AbstractValidator<B2CAccountTopUpRequest>
    {
        public B2CAccountTopUpValidator()
        {
            RuleFor(x => x.Initiator)
                .NotNull()
                .WithMessage("{PropertyName} - The name of the initiator is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - The name of the intiator should not be empty.");

            RuleFor(x => x.SecurityCredential)
                .NotNull()
                .WithMessage("{PropertyName} - The encrypted credential is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - The encrypted credential should not be empty.");

            RuleFor(x => x.QueueTimeOutUrl)
                .NotNull()
                .WithMessage("{PropertyName} - The queuetimeout url is required.")
                .Must(x => LinkMustBeAUri(x))
                .WithMessage("{PropertyName} - The queuetimeout url should be a valid secure url.");

            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage("{PropertyName} - Amount is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Amount must not be empty")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The amount should be in numeric value.");

            RuleFor(x => x.PartyA)
                .NotNull()
                .WithMessage("{PropertyName} - The shortcode is required.")
                .Length(5, 7)
                .WithMessage("{PropertyName} - The shortcode should be between 5 and 7 digit.");

            RuleFor(x => x.PartyB)
                .NotNull()
                .WithMessage("{PropertyName} - The account number is required.")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The account must be a numeric value.");

            RuleFor(x => x.ResultUrl)
                .NotNull()
                .WithMessage("{PropertyName} - The result url is required.")
                .Must(x => LinkMustBeAUri(x))
                .WithMessage("{PropertyName} - The result url should be a valid secure url.");

            RuleFor(x => x.AccountReference)
                .NotNull()
                .WithMessage("{PropertyName} - The account reference should not be empty.")
                .MaximumLength(12)
                .WithMessage("{PropertyName} - The account reference should not be more than 12 characters.");

            RuleFor(x => x.Remarks)
                .NotNull()
                .WithMessage("{PropertyName} - The remarks should not be empty.")
                .MaximumLength(100)
                .WithMessage("{PropertyName} - The remarks should not be more than 100 characters.");
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
