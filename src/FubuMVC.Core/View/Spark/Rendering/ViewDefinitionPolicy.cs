using FubuCore.Util;
using FubuMVC.Core.View.Spark.Registration;
using FubuMVC.Core.View.Spark.SparkModel;

namespace FubuMVC.Core.View.Spark.Rendering
{
    public interface IViewDefinitionPolicy
    {
        bool Matches(SparkDescriptor descriptor);
        ViewDefinition Create(SparkDescriptor descriptor);
    }

    public class DefaultViewDefinitionPolicy : IViewDefinitionPolicy
    {
        private readonly Cache<SparkDescriptor, ViewDefinition> _cache;

        public DefaultViewDefinitionPolicy()
        {
            _cache = new Cache<SparkDescriptor, ViewDefinition>(x => x.ToViewDefinition());
        }

        public bool Matches(SparkDescriptor descriptor)
        {
            return true;
        }

        public virtual ViewDefinition Create(SparkDescriptor descriptor)
        {
            return _cache[descriptor];
        }
    }
}