using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
    public class DynamicMpesaQRValidator : AbstractValidator<DynamicMpesaQR>
    {
        public DynamicMpesaQRValidator()
        {
            RuleFor(x => x.QRVersion)
                .NotNull()
                .WithMessage("{PropertyName} - Version number of QR is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Version number of QR should not be empty.");

            RuleFor(x => x.QRFormat)
                .NotNull()
                .WithMessage("{PropertyName} - Format of QR output is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - Format of QR output should not be empty.");

            RuleFor(x => x.QRType)
                .NotNull()
                .WithMessage("{PropertyName} - QR Type is required.")
                .NotEmpty()
                .WithMessage("{PropertyName} - QR Type should not be empty.");

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
