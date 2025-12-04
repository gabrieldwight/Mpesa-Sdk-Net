using FluentValidation;
using MpesaSdk.Dtos;

namespace MpesaSdk.Validators
{
	public class SwapRequestValidator : AbstractValidator<SwapRequest>
	{
		public SwapRequestValidator()
		{
			RuleFor(x => x.CustomerNumber)
				.NotNull()
				.WithMessage("{PropertyName} - The customer number is required.")
				.SetValidator(new PhoneNumberValidator<SwapRequest, string>())
				.WithMessage("{PropertyName} - The customer number should start with 2547XXXX.")
				.MaximumLength(12)
				.WithMessage("{PropertyName} - The customer number should be 12 digit.");
		}
	}
}
