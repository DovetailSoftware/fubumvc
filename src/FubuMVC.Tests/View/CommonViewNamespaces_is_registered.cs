using FubuMVC.Core.Registration;
using FubuMVC.Core.View;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Tests.View
{
    [TestFixture]
    public class CommonViewNamespaces_is_registered
    {
        [Test]
        public void is_registered()
        {
            var graph = BehaviorGraph.BuildFrom(x => {
                x.Import<CommonViewNamespacesRegistration>();
            });

            graph.Services.DefaultServiceFor<CommonViewNamespaces>()
                .ShouldNotBeNull();
        }
    }
}