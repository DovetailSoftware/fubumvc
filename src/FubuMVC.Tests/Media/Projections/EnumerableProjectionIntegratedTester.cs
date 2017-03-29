using System.Linq;
using System.Xml;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Media;
using FubuMVC.Core.Media.Projections;
using FubuMVC.Core.Media.Xml;
using FubuMVC.StructureMap;
using FubuTestingSupport;
using NUnit.Framework;
using StructureMap;

namespace FubuMVC.Tests.Media.Projections
{
    [TestFixture]
    public class EnumerableProjectionIntegratedTester
    {
        private Parent theParent;
        private IProjectionRunner runner;

        [SetUp]
        public void SetUp()
        {
            theParent = new Parent{
                Children = new Child[]{
                    new Child{Name = "Jeremy"},
                    new Child{Name = "Jessica"},
                    new Child{Name = "Natalie"}
                }
            };

            var container = new Container();
            var registry = new FubuRegistry();
            registry.Services<ResourcesServiceRegistry>();
            FubuApplication.For(registry).StructureMap(container).Bootstrap();

            runner = container.GetInstance<IProjectionRunner>();
        }

        [Test]
        public void accessors()
        {
            var projection = EnumerableProjection<Parent, Child>.For(x => x.Children);
            projection.As<IProjection<Parent>>().Accessors().Single().ShouldEqual(ReflectionHelper.GetAccessor<Parent>(x => x.Children));
        }

        public XmlElement write(IProjection<Parent> projection)
        {
            var node = XmlNodeCentricMediaNode.ForRoot("root");
            runner.Run(projection, new SimpleValues<Parent>(theParent), node);

            return node.Element;
        }

        [Test]
        public void write_with_inline_projection()
        {
            var projection = new Projection<Parent>(DisplayFormatting.RawValues);
            projection.Enumerable(x => x.Children).DefineProjection(p =>
            {
                p.Value(x => x.Name).Name("name");
            });

            var element = write(projection);

            element.OuterXml.ShouldEqual("<root><Children><Child><name>Jeremy</name></Child><Child><name>Jessica</name></Child><Child><name>Natalie</name></Child></Children></root>");
        }

        [Test]
        public void write_with_precanned_child_projection_with_defaults()
        {
            var projection = new Projection<Parent>(DisplayFormatting.RawValues);
            projection.Enumerable(x => x.Children).UseProjection<SimpleChildProjection>();

            var element = write(projection);

            element.OuterXml.ShouldEqual("<root><Children><Child><name>Jeremy</name></Child><Child><name>Jessica</name></Child><Child><name>Natalie</name></Child></Children></root>");
        }

        [Test]
        public void write_with_precanned_child_projection_overwrite_node()
        {
            var projection = new Projection<Parent>(DisplayFormatting.RawValues);
            projection.Enumerable(x => x.Children).NodeName("children").UseProjection<SimpleChildProjection>();

            var element = write(projection);

            element.OuterXml.ShouldEqual("<root><children><Child><name>Jeremy</name></Child><Child><name>Jessica</name></Child><Child><name>Natalie</name></Child></children></root>");
        }

        [Test]
        public void write_with_precanned_child_projection_overwrite_leaf_name()
        {
            var projection = new Projection<Parent>(DisplayFormatting.RawValues);
            projection.Enumerable(x => x.Children).LeafName("child").UseProjection<SimpleChildProjection>();

            var element = write(projection);

            element.OuterXml.ShouldEqual("<root><Children><child><name>Jeremy</name></child><child><name>Jessica</name></child><child><name>Natalie</name></child></Children></root>");
        }

        public class Parent
        {
            public Child[] Children { get; set; }
        }

        public class SimpleChildProjection : Projection<Child>
        {
            public SimpleChildProjection()
                : base(DisplayFormatting.RawValues)
            {
                Value(x => x.Name).Name("name");
            }
        }

        public class Child
        {
            public string Name { get; set; }
        }
    }
}