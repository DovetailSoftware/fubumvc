using Spark;

namespace FubuMVC.Core.View.Spark.Rendering
{
    public class ViewDefinition
    {
        public ViewDefinition(SparkViewDescriptor viewDescriptor, SparkViewDescriptor partialDescriptor)
        {
            ViewDescriptor = viewDescriptor;
            PartialDescriptor = partialDescriptor;
        }
        public SparkViewDescriptor ViewDescriptor { get; private set; }
        public SparkViewDescriptor PartialDescriptor { get; private set; }
    }
}