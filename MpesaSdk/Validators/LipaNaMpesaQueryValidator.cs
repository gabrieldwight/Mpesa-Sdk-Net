using FluentValidation;
using MpesaSdk.Dtos;

namespace MpesaSdk.Validators
{
    public class LipaNaMpesaQueryValidator : AbstractValidator<LipaNaMpesaQuery>
    {
        public LipaNaMpesaQueryValidator()
        {
            int i = 0;
            RuleFor(x => x.BusinessShortCode).NotNull().Must(x => int.TryParse(x, out i)).Length(5, 7);
            RuleFor(x => x.CheckoutRequestID).NotNull().NotEmpty();
        }
    }
}
