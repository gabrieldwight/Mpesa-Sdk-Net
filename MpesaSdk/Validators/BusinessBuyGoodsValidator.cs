using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
	public class BusinessBuyGoodsValidator : AbstractValidator<BusinessBuyGoodsRequest>
	{
		public BusinessBuyGoodsValidator() 
		{
			RuleFor(x => x.Initiator)
				.NotNull()
				.WithMessage("{PropertyName} - The name of the initiator is required.")
				.NotEmpty()
				.WithMessage("{PropertyName} - The name of the initiator should not be empty.");

			RuleFor(x => x.SecurityCredential)
				.NotNull()
				.WithMessage("{PropertyName} - The encrypted credential is required.")
				.NotEmpty()
				.WithMessage("{PropertyName} - The encrypted credential should not be empty.");

			RuleFor(x => x.Amount)
				.NotNull()
				.WithMessage("{PropertyName} - Amount is required.")
				.NotEmpty()
				.WithMessage("{PropertyName} - Amount must not be empty")
				.Must(x => int.TryParse(x, out int value))
				.WithMessage("{PropertyName} - The amount should be in numeric value.");

			RuleFor(x => x.AccountReference)
			   .NotNull()
			   .WithMessage("{PropertyName} - The account number to be associated with the payment. Up to 13 characters is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - The account number to be associated with the payment. Up to 13 characters should not be empty.");

			RuleFor(x => x.PartyA)
				.NotNull()
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode is required.")
				.Must(x => int.TryParse(x, out int value))
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode should be in numeric value.")
				.Length(5, 7)
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode should be 5 to 7 digits account number.");

			RuleFor(x => x.PartyB)
				.NotNull()
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode is required.")
				.Must(x => int.TryParse(x, out int value))
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode should be in numeric value.")
				.Length(5, 7)
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode should be 5 to 7 digits account number.");

			RuleFor(x => x.Requester)
			   .NotNull()
			   .WithMessage("{PropertyName} - The consumer’s mobile number on behalf of whom you are paying is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - The consumer’s mobile number on behalf of whom you are paying should not be empty.");

			RuleFor(x => x.Remarks)
			   .NotNull()
			   .WithMessage("{PropertyName} - Any additional information associated with the transaction is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - Any additional information associated with the transaction should not be empty.");

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
