using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
    public class LipaNaMpesaOnlineValidator : AbstractValidator<LipaNaMpesaOnline>
    {
        public LipaNaMpesaOnlineValidator()
        {
            int i = 0;

            RuleFor(x => x.BusinessShortCode).NotNull().Must(x => int.TryParse(x, out i)).Length(5, 7);
            RuleFor(x => x.Amount).NotNull().NotEmpty().Must(x => int.TryParse(x, out i));
            RuleFor(x => x.PartyA).NotNull().SetValidator(new PhoneNumberValidator()).MaximumLength(12);
            RuleFor(x => x.PartyB).NotNull().Must(x => int.TryParse(x, out i)).Length(5, 7);
            RuleFor(x => x.PhoneNumber).NotNull().SetValidator(new PhoneNumberValidator()).MaximumLength(12);
            RuleFor(x => x.CallBackURL).NotNull().Must(x => LinkMustBeAUri(x));
            RuleFor(x => x.AccountReference).NotNull().MaximumLength(12);
            RuleFor(x => x.TransactionDesc).NotNull().MaximumLength(13);
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
