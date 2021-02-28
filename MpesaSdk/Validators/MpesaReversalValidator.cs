using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
    public class MpesaReversalValidator : AbstractValidator<MpesaReversal>
    {
        public MpesaReversalValidator()
        {
            RuleFor(x => x.ReceiverParty)
                .NotNull()
                .WithMessage("{PropertyName} - The paybill or till number of the organization receiving the transaction should not be empty.")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The paybill or till number of the organization receiving the transaction must be a numeric value.")
                .Length(5, 7)
                .WithMessage("{PropertyName} - The paybill or till number of the organization receiving the transaction should be 5 to 7 account number digit.");


            RuleFor(x => x.RecieverIdentifierType)
                .NotNull()
                .WithMessage("{PropertyName} - Organization identifier receiving the transaction is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Organization identifier receiving the transaction should not be empty.");


            RuleFor(x => x.Remarks)
                .NotNull()
                .WithMessage("{PropertyName} - Reason for reversal is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Reason for reversal should not be empty");

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

            RuleFor(x => x.TransactionID)
                .NotNull()
                .WithMessage("{PropertyName} - The Mpesa transactionID is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - The Mpesa transactionID should not be empty.");
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
