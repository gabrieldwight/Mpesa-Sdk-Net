using FluentValidation;
using FluentValidation.Validators;

namespace MpesaSdk.Validators
{
    public class PhoneNumberValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name
        {
            get
            {
                return "PhoneNumberValidator";
            }
        }

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            var phoneNumberValue = value as string;

            if (!phoneNumberValue?.StartsWith("254") ?? false)
            {
                return false;
            }
            return true;
        }
    }
}
