using FubuMVC.Core;
using FubuMVC.Core.Media;
using FubuMVC.Core.Media.Projections;
using FubuMVC.Core.Registration;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Tests.Media
{
    [TestFixture]
    public class default_service_registrations
    {
        private ServiceGraph theServices;

        [SetUp]
        public void SetUp()
        {
            var registry = new FubuRegistry();
            registry.Services<ResourcesServiceRegistry>();

            theServices = BehaviorGraph.BuildFrom(registry).Services;
        }
        
        [Test]
        public void value_source_is_registered()
        {
            theServices.DefaultServiceFor(typeof (IValueSource<>)).Type.ShouldEqual(typeof (ValueSource<>));
        }

        [Test]
        public void values_is_registered()
        {
            theServices.DefaultServiceFor(typeof (IValues<>)).Type.ShouldEqual(typeof (SimpleValues<>));
        }



        [Test]
        public void projection_runner_is_registered()
        {
            theServices.DefaultServiceFor(typeof (IProjectionRunner)).Type.ShouldEqual(typeof (ProjectionRunner));
        }

        [Test]
        public void generic_projection_runner_is_registered()
        {
            theServices.DefaultServiceFor(typeof(IProjectionRunner<>)).Type.ShouldEqual(typeof(ProjectionRunner<>));
        }
         
    }
}