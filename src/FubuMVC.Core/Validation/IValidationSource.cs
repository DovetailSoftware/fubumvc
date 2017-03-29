using System;
using System.Collections.Generic;

namespace FubuMVC.Core.Validation
{
    public interface IValidationSource
    {
        IEnumerable<IValidationRule> RulesFor(Type type);
    }
}