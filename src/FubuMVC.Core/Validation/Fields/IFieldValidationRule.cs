using FubuCore.Reflection;
using FubuLocalization;

namespace FubuMVC.Core.Validation.Fields
{
    public interface IFieldValidationRule
    {
		StringToken Token { get; set; }

        void Validate(Accessor accessor, ValidationContext context);
    }
}