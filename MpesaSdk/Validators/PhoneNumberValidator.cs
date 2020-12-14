using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MpesaSdk.Validators
{
    public class PhoneNumberValidator : PropertyValidator
    {
        public PhoneNumberValidator() : base()
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var phoneNumberValue = context.PropertyValue as string;

            if (!phoneNumberValue.StartsWith("254"))
            {
                return false;
            }
            return true;
        }
    }
}
