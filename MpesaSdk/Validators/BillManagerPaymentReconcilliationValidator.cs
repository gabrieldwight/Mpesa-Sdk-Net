using FluentValidation;
using MpesaSdk.Dtos;

namespace MpesaSdk.Validators
{
	public class BillManagerPaymentReconcilliationValidator : AbstractValidator<BillManagerPaymentReconcilliationRequest>
	{
		public BillManagerPaymentReconcilliationValidator() 
		{
			RuleFor(x => x.TransactionId)
			   .NotNull()
			   .WithMessage("{PropertyName} - Transaction ID is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - Transaction ID should not be empty.");

			RuleFor(x => x.PaidAmount)
				.NotNull()
				.WithMessage("{PropertyName} - Amount is required.")
				.NotEmpty()
				.WithMessage("{PropertyName} - Amount must not be empty")
				.Must(x => int.TryParse(x, out int value))
				.WithMessage("{PropertyName} - The amount should be in numeric value.");

			RuleFor(x => x.Msisdn)
			   .NotNull()
			   .WithMessage("{PropertyName} - Phone number is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - Phone number should not be empty.");

			RuleFor(x => x.DateCreated)
			   .NotNull()
			   .WithMessage("{PropertyName} - Transaction Date is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - Transaction Date should not be empty.");

			RuleFor(x => x.AccountReference)
			   .NotNull()
			   .WithMessage("{PropertyName} - Account Reference is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - Account Reference should not be empty.");

			RuleFor(x => x.ShortCode)
				.NotNull()
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode is required.")
				.Must(x => int.TryParse(x, out int value))
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode should be in numeric value.")
				.Length(5, 7)
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode should be 5 to 7 digits account number.");
		}
	}
}
