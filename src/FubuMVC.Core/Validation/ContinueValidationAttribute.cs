using System.Collections.Generic;
using System.Reflection;
using FubuMVC.Core.Validation.Fields;

namespace FubuMVC.Core.Validation
{
    public class ContinueValidationAttribute : FieldValidationAttribute
    {
        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new ContinuationFieldRule();
        }
    }
}