using FubuMVC.Core.View.Spark;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Tests.View.Spark
{
    [TestFixture]
    public class SparkEngineSettingsTester : InteractionContext<SparkEngineSettings>
    {
        [Test]
        public void includes_spark_views_and_bindings_by_default()
        {
            ClassUnderTest.Search.Include.Split(';')
                .ShouldHaveTheSameElementsAs("*.spark", "*.shade", "bindings.xml");
        }

        [Test]
        public void uses_deep_search_by_default()
        {
            ClassUnderTest.Search.DeepSearch.ShouldBeTrue();
        }
    }
}
