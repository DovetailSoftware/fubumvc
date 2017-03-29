using FubuMVC.Core.Media.Projections;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Tests.Media.Projections
{
    [TestFixture]
    public class ValueSourceTester
    {
        [Test]
        public void find_values_invokes_the_fubu_request()
        {
            var request = new InMemoryFubuRequest();
            var address = new Xml.Address();

            request.Set(address);

            var source = new ValueSource<Xml.Address>(request);

            source.FindValues().ShouldBeOfType<SimpleValues<Xml.Address>>()
                .Subject.ShouldBeTheSameAs(address);
        }
    }
}