using FluentValidation;
using MpesaSdk.Dtos;
using System;

namespace MpesaSdk.Validators
{
    public class BusinessToCustomerValidator : AbstractValidator<BusinessToCustomer>
    {
        public BusinessToCustomerValidator()
        {
            int i = 0;
            RuleFor(x => x.InitiatorName).NotNull().NotEmpty();
            RuleFor(x => x.SecurityCredential).NotNull().NotEmpty();
            RuleFor(x => x.Amount).NotNull().NotEmpty().Must(x => int.TryParse(x, out i));
            RuleFor(x => x.PartyA).NotNull().Must(x => int.TryParse(x, out i)).Length(5, 7);
            RuleFor(x => x.PartyB).NotNull().Must(x => int.TryParse(x, out i))
                .SetValidator(new PhoneNumberValidator()).MaximumLength(12);            
            RuleFor(x => x.Remarks).NotNull().NotEmpty();
            RuleFor(x => x.QueueTimeOutURL).NotNull().Must(x => LinkMustBeAUri(x));
            RuleFor(x => x.ResultURL).NotNull().Must(x => LinkMustBeAUri(x));
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
