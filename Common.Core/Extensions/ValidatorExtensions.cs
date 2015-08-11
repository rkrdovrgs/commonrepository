using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Core.Extensions
{

    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TElement> IsValidUri<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new IsValidUriItemValidator<TElement>());
        }
    }




    public class IsValidUriItemValidator<T> : PropertyValidator
    {

        public IsValidUriItemValidator()
            : base("'{PropertyName}' is not a valid URI")
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null) return false;
            return Uri.IsWellFormedUriString(context.PropertyValue.ToString(), UriKind.Absolute);
        }
    }
}
