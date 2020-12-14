using FluentValidation;
using MpesaSdk.Dtos;

namespace MpesaSdk.Validators
{
    public class CustomerToBusinessSimulateTransactionValidator : AbstractValidator<CustomerToBusinessSimulate>
    {
        public CustomerToBusinessSimulateTransactionValidator()
        {
            int i = 0;

            RuleFor(x => x.ShortCode).NotNull().Must(x => int.TryParse(x, out i)).Length(5, 7);
            RuleFor(x => x.Amount).NotNull().NotEmpty().Must(x => int.TryParse(x, out i));
            RuleFor(x => x.Msisdn).NotNull().Must(x => int.TryParse(x, out i))
                .SetValidator(new PhoneNumberValidator()).MaximumLength(12);
            RuleFor(x => x.BillRefNumber).NotNull().NotEmpty();
        }
    }
}
