using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
    public class CustomerToBusinessSimulateTransactionValidator : AbstractValidator<CustomerToBusinessSimulate>
    {
        public CustomerToBusinessSimulateTransactionValidator()
        {
            RuleFor(x => x.ShortCode)
                .NotNull()
                .WithMessage("{PropertyName} - The receiving organization paybill or till number shortcode is required.")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The receiving organization paybill or till number must be a numeric value.")
                .Length(5, 7)
                .WithMessage("{PropertyName} - The receiving organization paybill or till number should be 5 to 7 digits account number.");

            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage("{PropertyName} - Amount is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Amount must not be empty")
                .Must(x => int.TryParse(x, out int value))
                .WithMessage("{PropertyName} - The amount should be in numeric value.");

            RuleFor(x => x.Msisdn)
                .NotNull()
                .WithMessage("{PropertyName} - The mobile number is required.")
                .SetValidator(new PhoneNumberValidator())
                .WithMessage("{PropertyName} - The mobile number should start with 2547XXXX.")
                .MaximumLength(12)
                .WithMessage("{PropertyName} - The mobile number should be 12 digit.");

            RuleFor(x => x.BillRefNumber)
                .NotNull()
                .WithMessage("{PropertyName} - The account number is required..")
                .NotEmpty()
                .WithMessage("{PropertyName} - The account number should not be empty.")
                .When(x => x.CommandID.Equals(Transaction_Type.CustomerPayBillOnline, StringComparison.OrdinalIgnoreCase));
        }
    }
}
