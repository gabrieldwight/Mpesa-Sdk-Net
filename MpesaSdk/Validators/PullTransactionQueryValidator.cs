using FluentValidation;
using MpesaSdk.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MpesaSdk.Validators
{
    public class PullTransactionQueryValidator : AbstractValidator<PullTransactionQuery>
    {
        public PullTransactionQueryValidator()
        {
            RuleFor(x => x.ShortCode)
               .NotNull()
               .WithMessage("{PropertyName} - The paybill or till number should not be empty.")
               .Must(x => int.TryParse(x, out int value))
               .WithMessage("{PropertyName} - The paybill or till number must be a numeric value.")
               .Length(5, 7)
               .WithMessage("{PropertyName} - The paybill or till number should be 5 to 7 account number.");

            RuleFor(x => x.StartDate)
                .NotNull()
                .WithMessage("{PropertyName} - The start date should not be empty.");

            RuleFor(x => x.EndDate)
                .NotNull()
                .WithMessage("{PropertyName} - The end date should not be empty.");

            RuleFor(x => x.OffSetValue)
                .NotNull()
                .WithMessage("{PropertyName} - The offset value should not be empty.");
        }
    }
}
