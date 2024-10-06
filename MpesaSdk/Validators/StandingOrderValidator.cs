using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
	public class StandingOrderValidator : AbstractValidator<StandingOrderRequest>
	{
		public StandingOrderValidator() 
		{
			RuleFor(x => x.StandingOrderName)
				.NotNull()
				.WithMessage("{PropertyName} - The Standing order name is required")
				.NotEmpty()
				.WithMessage("{PropertyName} - The Standing order name must not be empty");

			RuleFor(x => x.StartDate)
				.NotNull()
				.WithMessage("{PropertyName} - The start date is required")
				.NotEmpty()
				.WithMessage("{PropertyName} - The start date must not be empty");

			RuleFor(x => x.EndDate)
				.NotNull()
				.WithMessage("{PropertyName} - The end date is required")
				.NotEmpty()
				.WithMessage("{PropertyName} - The end date must not be empty");

			RuleFor(x => x.TransactionType)
				.NotNull()
				.WithMessage("{PropertyName} - The Transaction type is required")
				.NotEmpty()
				.WithMessage("{PropertyName} - The Transaction type must not be empty");

			RuleFor(x => x.BusinessShortCode)
				.NotNull()
				.WithMessage("{PropertyName} - The paybill or till number shortcode should not be empty.")
				.Must(x => int.TryParse(x, out int value))
				.WithMessage("{PropertyName} - The paybill or till number must be a numeric value.")
				.Length(5, 7)
				.WithMessage("{PropertyName} - The paybill or till number should be 5 to 7 account number digits.");

			RuleFor(x => x.Amount)
				.NotNull()
				.WithMessage("{PropertyName} - Amount is required.")
				.NotEmpty()
				.WithMessage("{PropertyName} - Amount must not be empty")
				.Must(x => int.TryParse(x, out int value))
				.WithMessage("{PropertyName} - The amount should be in numeric value.");

			RuleFor(x => x.PartyA)
				.NotNull()
				.WithMessage("{PropertyName} - The mobile number is required.")
				.SetValidator(new PhoneNumberValidator<StandingOrderRequest, string>())
				.WithMessage("{PropertyName} - The mobile number should start with 2547XXXX.")
				.MaximumLength(12)
				.WithMessage("{PropertyName} - The mobile number should be 12 digit.");

			RuleFor(x => x.CallBackUrl)
				.NotNull()
				.WithMessage("{PropertyName} - The callback url is required.")
				.Must(x => LinkMustBeAUri(x))
				.WithMessage("{PropertyName} - The callback url should be a valid secure url.");

			RuleFor(x => x.AccountReference)
				.NotNull()
				.WithMessage("{PropertyName} - The account reference should not be empty.")
				.MaximumLength(12)
				.WithMessage("{PropertyName} - The account reference should not be more than 12 characters.");

			RuleFor(x => x.TransactionDesc)
				.NotNull()
				.WithMessage("{PropertyName} - The transaction description should not be empty.")
				.MaximumLength(13)
				.WithMessage("{PropertyName} - The transaction description should not be more than 13 characters.");

			RuleFor(x => x.Frequency)
				.NotNull()
				.WithMessage("{PropertyName} - The Frequency is required")
				.NotEmpty()
				.WithMessage("{PropertyName} - The Frequency must not be empty");

			RuleFor(x => x.ReceiverPartyIdentifierType)
				.NotNull()
				.WithMessage("{PropertyName} - The Receiver party identifier type is required")
				.NotEmpty()
				.WithMessage("{PropertyName} - The Receiver party identifier type must not be empty")
				.NotEqual("1")
				.WithMessage("{PropertyName} - The Receiver party does not support MSISDN");
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
