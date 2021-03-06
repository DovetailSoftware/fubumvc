using System.Linq;
using FubuCore;
using FubuCore.Formatting;
using FubuCore.Reflection;
using FubuMVC.Core.Media.Projections;
using FubuMVC.Core.Media.Xml;
using FubuMVC.Core.Urls;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.Media.Xml
{
    [TestFixture]
    public class AccessorProjectionTester
    {
        private AccessorProjection<ValueTarget, int> theAccessorProjection;
        private SimpleValues<ValueTarget> _theValues;
        private XmlAttCentricMediaNode theMediaNode;

        [SetUp]
        public void SetUp()
        {
            theAccessorProjection = AccessorProjection<ValueTarget, int>.For(x => x.Age);
            _theValues = new SimpleValues<ValueTarget>(new ValueTarget
            {
                Age = 37
            });

            theMediaNode = XmlAttCentricMediaNode.ForRoot("root");
        }

        [Test]
        public void accessors()
        {
            theAccessorProjection.As<IProjection<ValueTarget>>().Accessors()
                                 .Single()
                                 .ShouldEqual(ReflectionHelper.GetAccessor<ValueTarget>(x => x.Age));
        }

        [Test]
        public void project_the_property_with_default_node_name()
        {
            theAccessorProjection.As<IProjection<ValueTarget>>().Write(new ProjectionContext<ValueTarget>(null, _theValues), theMediaNode);

            theMediaNode.Element.GetAttribute("Age").ShouldEqual("37");
        }

        [Test]
        public void project_the_property_with_a_different_node_name()
        {
            theAccessorProjection.Name("CurrentAge");

            theAccessorProjection.As<IProjection<ValueTarget>>().Write(new ProjectionContext<ValueTarget>(null, _theValues), theMediaNode);

            theMediaNode.Element.GetAttribute("CurrentAge").ShouldEqual("37");
        }

        [Test]
        public void project_the_property_with_formatting()
        {
            theAccessorProjection.FormattedBy(age => "*" + age + "*");
            theAccessorProjection.As<IProjection<ValueTarget>>().Write(new ProjectionContext<ValueTarget>(null, _theValues), theMediaNode);

            theMediaNode.Element.GetAttribute("Age").ShouldEqual("*37*");
        }

        [Test]
        public void project_the_property_with_formatting_thru_display_formatter()
        {
            var services = MockRepository.GenerateMock<IServiceLocator>();
            var formatter = MockRepository.GenerateMock<IDisplayFormatter>();
            services.Stub(x => x.GetInstance<IDisplayFormatter>()).Return(formatter);

            var accessor = ReflectionHelper.GetAccessor<ValueTarget>(x => x.Age);
            var theFormattedValue = "*37*";

            formatter.Stub(x => x.GetDisplayForValue(accessor, 37)).Return(theFormattedValue);

            theAccessorProjection.Formatted();

            theAccessorProjection.As<IProjection<ValueTarget>>().Write(new ProjectionContext<ValueTarget>(services, _theValues), theMediaNode);

            theMediaNode.Element.GetAttribute("Age").ShouldEqual(theFormattedValue);
        }

        [Test]
        public void write_url_for_model()
        {
            var urls = new StubUrlRegistry();
            var services = MockRepository.GenerateMock<IServiceLocator>();
            services.Stub(x => x.GetInstance<IUrlRegistry>()).Return(urls);

            theAccessorProjection.WriteUrlFor(age => new CreateValueTarget{
                Name = age.ToString()
            });

            var expectedUrl = urls.UrlFor(new CreateValueTarget{
                Name = "37"
            });

            theAccessorProjection.As<IProjection<ValueTarget>>().Write(new ProjectionContext<ValueTarget>(services, _theValues), theMediaNode);

            theMediaNode.Element.GetAttribute("Age").ShouldEqual(expectedUrl);
        }
    }

    public class ValueTarget
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class CreateValueTarget
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("CreateValueTarget: {0}", Name);
        }
    }
}