using StructureMap.Configuration.DSL;

namespace FubuMVC.Core.Validation
{
    public static class ValidationBootstrapExtensions
    {
        public static void FubuValidation(this Registry registry)
        {
            registry.IncludeRegistry<FubuValidationRegistry>();
        }
    }
}