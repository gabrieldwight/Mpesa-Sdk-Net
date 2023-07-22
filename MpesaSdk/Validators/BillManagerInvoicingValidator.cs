using FluentValidation;
using MpesaSdk.Dtos;

namespace MpesaSdk.Validators
{
	public class BillManagerInvoicingValidator : AbstractValidator<BillManagerInvoicingRequest>
	{
		public BillManagerInvoicingValidator() 
		{
			RuleFor(x => x.ExternalReference)
			   .NotNull()
			   .WithMessage("{PropertyName} - This is a unique invoice name on your system’s end is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - This is a unique invoice name on your system’s end should not be empty.");

			RuleFor(x => x.BilledFullName)
			   .NotNull()
			   .WithMessage("{PropertyName} - The name of the recipient to receive the invoice details is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - The name of the recipient to receive the invoice details should not be empty.");

			RuleFor(x => x.BilledPhoneNumber)
			   .NotNull()
			   .WithMessage("{PropertyName} - This is the phone number to receive invoice details via sms is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - This is the phone number to receive invoice details via sms should not be empty.");

			RuleFor(x => x.BilledPeriod)
			   .NotNull()
			   .WithMessage("{PropertyName} - Bill Date is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - Bill Date should not be empty.");

			RuleFor(x => x.DueDate)
			   .NotNull()
			   .WithMessage("{PropertyName} - This is the date you expect the customer to have paid the invoice amount is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - This is the date you expect the customer to have paid the invoice amount should not be empty.");

			RuleFor(x => x.AccountReference)
			   .NotNull()
			   .WithMessage("{PropertyName} - The account number being invoiced that uniquely identifies a customer is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - The account number being invoiced that uniquely identifies a customer should not be empty.");

			RuleFor(x => x.Amount)
				.NotNull()
				.WithMessage("{PropertyName} - Amount is required.")
				.NotEmpty()
				.WithMessage("{PropertyName} - Amount must not be empty")
				.Must(x => int.TryParse(x, out int value))
				.WithMessage("{PropertyName} - The amount should be in numeric value.");

			RuleForEach(x => x.InvoiceItems).SetValidator(new InvoiceItemsValidator());
		}
	}

	internal class InvoiceItemsValidator : AbstractValidator<InvoiceItem>
	{
		internal InvoiceItemsValidator()
		{
			RuleFor(x => x.ItemName)
			   .NotNull()
			   .WithMessage("{PropertyName} - Item Name is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - Item Name should not be empty.");

			RuleFor(x => x.Amount)
				.NotNull()
				.WithMessage("{PropertyName} - Amount is required.")
				.NotEmpty()
				.WithMessage("{PropertyName} - Amount must not be empty")
				.Must(x => int.TryParse(x, out int value))
				.WithMessage("{PropertyName} - The amount should be in numeric value.");
		}
	}
}
