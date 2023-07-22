using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
	public class BillManagerOnboardingValidator : AbstractValidator<BillManagerOnboardingRequest>
	{
		public BillManagerOnboardingValidator() 
		{
			RuleFor(x => x.Shortcode)
				.NotNull()
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode is required.")
				.Must(x => int.TryParse(x, out int value))
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode should be in numeric value.")
				.Length(5, 7)
				.WithMessage("{PropertyName} - The organization paybill/till number shortcode should be 5 to 7 digits account number.");

			RuleFor(x => x.Email)
			   .NotNull()
			   .WithMessage("{PropertyName} - This is the official contact email address for the organization signing up to bill manager is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - This is the official contact email address for the organization signing up to bill manager should not be empty.");

			RuleFor(x => x.OfficialContact)
			   .NotNull()
			   .WithMessage("{PropertyName} - This is the official contact phone number for the organization signing up to bill manager is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - This is the official contact phone number for the organization signing up to bill manager should not be empty.");

			RuleFor(x => x.SendReminders)
			   .NotNull()
			   .WithMessage("{PropertyName} - This field gives you the flexibility as a business to enable or disable sms payment reminders for invoices sent. is required.")
			   .NotEmpty()
			   .WithMessage("{PropertyName} - This field gives you the flexibility as a business to enable or disable sms payment reminders for invoices sent. should not be empty.");

			RuleFor(x => x.Callbackurl)
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
