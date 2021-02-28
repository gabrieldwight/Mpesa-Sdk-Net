using FluentValidation.Validators;

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

            if (!phoneNumberValue?.StartsWith("254") ?? false)
            {
                return false;
            }
            return true;
        }
    }
}
