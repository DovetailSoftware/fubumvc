using FubuCore.Reflection;
using FubuLocalization;

namespace FubuMVC.Core.Validation.Fields
{
    public class ContinuationFieldRule : IFieldValidationRule
    {
	    public StringToken Token { get; set; }

	    public void Validate(Accessor accessor, ValidationContext context)
        {
            context.ContinueValidation(accessor);
        }
    }
}