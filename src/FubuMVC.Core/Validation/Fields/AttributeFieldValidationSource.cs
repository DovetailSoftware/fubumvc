using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FubuCore.Reflection;

namespace FubuMVC.Core.Validation.Fields
{
#pragma warning disable 659
    public class AttributeFieldValidationSource : IFieldValidationSource
#pragma warning restore 659
    {
        public IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            return property.GetAllAttributes<FieldValidationAttribute>()
                .SelectMany(x => x.RulesFor(property));
        }

        public void AssertIsValid()
        {
            
        }


        public override bool Equals(object obj)
        {
            return obj is AttributeFieldValidationSource;
        }
    }
}