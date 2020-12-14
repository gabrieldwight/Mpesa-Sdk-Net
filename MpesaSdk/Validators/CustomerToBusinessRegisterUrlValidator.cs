using FluentValidation;
using MpesaSdk.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MpesaSdk.Validators
{
    public class CustomerToBusinessRegisterUrlValidator : AbstractValidator<CustomerToBusinessRegisterUrl>
    {
        public CustomerToBusinessRegisterUrlValidator()
        {
            int i = 0;

            RuleFor(x => x.ShortCode).NotNull().Must(x => int.TryParse(x, out i)).Length(5, 7);
            RuleFor(x => x.ResponseType).NotNull().NotEmpty();
            RuleFor(x => x.ConfirmationURL).NotNull().Must(x => LinkMustBeAUri(x));
            RuleFor(x => x.ValidationURL).NotNull().Must(x => LinkMustBeAUri(x));
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
