using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Media;
using FubuMVC.Core.Media.Projections;
using FubuMVC.Core.Media.Xml;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime.Formatters;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Tests.Media
{
    [TestFixture]
    public class Resource_integrated_registration_tester
    {
        private BehaviorGraph theBehaviorGraph;
        private ConnegGraph theConnegGraph;

        [SetUp]
        public void SetUp()
        {
            var registry = new FubuRegistry();
            registry.Actions.IncludeType<RestController1>();

            // TODO -- this really needs to be done with a Bottle
            var typePool = new TypePool();
            typePool.AddAssembly(Assembly.GetExecutingAssembly());

            registry.Policies.Add(new ConnegAttachmentPolicy(typePool));
            registry.Services<ResourcesServiceRegistry>();


            registry.Policies.Add(x => {
                x.Where.AnyActionMatches(
                    call => call.HandlerType == typeof (RestController1) && !call.Method.Name.StartsWith("Not"));

                x.Conneg.ApplyConneg();
            });

            theBehaviorGraph = BehaviorGraph.BuildFrom(registry);
            theConnegGraph = new ConnegGraph(theBehaviorGraph);
        }

        [Test]
        public void resource_also_references_the_projection()
        {
            // A resource is applied by the policy
            theBehaviorGraph.Services.ServicesFor<IProjection<Xml.Address>>().Single()
                .ShouldNotBeNull();
        }

        [Test]
        public void resource_also_registers_the_link_source_configuration()
        {
            theBehaviorGraph.Services.ServicesFor<ILinkSource<Xml.Address>>().Single()
                .ShouldNotBeNull();
        }

        [Test]
        public void conneg_graph_can_find_all_the_output_nodes_for_a_type()
        {
            theConnegGraph.OutputNodesFor<Xml.Address>().Where(x => x.ParentChain() != null).Select(x => x.Previous.As<ActionCall>().Method.Name)
                .ShouldHaveTheSameElementsAs("Find", "put_city", "Method3");

            theConnegGraph.OutputNodesFor<Output1>().Where(x => x.ParentChain() != null).Select(x => x.Previous.As<ActionCall>().Method.Name)
                .ShouldHaveTheSameElementsAs("Method4");
        }


        [Test]
        public void conneg_graph_can_find_all_input_nodes_for_a_type()
        {
            var nodes = theConnegGraph.InputNodesFor<Input1>().Where(x => x.ParentChain() != null);

            nodes.Each(x => Debug.WriteLine(x.Next.As<ActionCall>().Method.Name));


            nodes.Select(x => x.Next.As<ActionCall>().Method.Name)
                .ShouldHaveTheSameElementsAs("Find", "Method4");
        }

        [Test]
        public void just_having_a_resource_turns_the_formatters_to_none()
        {
            // Default condition first, before the resource is applied
            var theChain = theBehaviorGraph.BehaviorFor<RestController1>(x => x.Find(null));
            theChain.RemoveConneg();
            theChain.ApplyConneg();
            theChain.Output.UsesFormatter<JsonFormatter>().ShouldBeTrue();
            theChain.Output.UsesFormatter<XmlFormatter>().ShouldBeTrue();

            theConnegGraph = new ConnegGraph(theBehaviorGraph);
        
            // Now apply a resource
            new Resource<Xml.Address>().As<IResourceRegistration>().Modify(theConnegGraph, theBehaviorGraph);

            // Default condition first, before the resource is applied
            theChain.Output.UsesFormatter<JsonFormatter>().ShouldBeFalse();
            theChain.Output.UsesFormatter<XmlFormatter>().ShouldBeFalse();
        }

        [Test]
        public void simple_resource_applies_json_formatter()
        {
            var resource = new Resource<Xml.Address>();
            resource.SerializeToJson();

            resource.As<IResourceRegistration>().Modify(theConnegGraph, theBehaviorGraph);

            var outputNode = theBehaviorGraph.BehaviorFor<RestController1>(x => x.Find(null)).Output;
            outputNode.UsesFormatter<JsonFormatter>().ShouldBeTrue();
            outputNode.UsesFormatter<XmlFormatter>().ShouldBeFalse();


            theBehaviorGraph.BehaviorFor<RestController1>(x => x.put_city(null))
                .Output.UsesFormatter<JsonFormatter>().ShouldBeTrue();

            theBehaviorGraph.BehaviorFor<RestController1>(x => x.Method3(null))
                .Output.UsesFormatter<JsonFormatter>().ShouldBeTrue();
        }

        [Test]
        public void simple_resource_applies_xml_formatter()
        {
            var resource = new Resource<Xml.Address>();
            resource.SerializeToXml();

            resource.As<IResourceRegistration>().Modify(theConnegGraph, theBehaviorGraph);

            theBehaviorGraph.BehaviorFor<RestController1>(x => x.Find(null))
                .Output.UsesFormatter<XmlFormatter>().ShouldBeTrue();


            theBehaviorGraph.BehaviorFor<RestController1>(x => x.put_city(null))
                .Output.UsesFormatter<XmlFormatter>().ShouldBeTrue();

            theBehaviorGraph.BehaviorFor<RestController1>(x => x.Method3(null))
                .Output.UsesFormatter<XmlFormatter>().ShouldBeTrue();
        }

        [Test]
        public void create_a_media_write_node_for_xml()
        {
            var resource = new Resource<Xml.Address>();
            XmlMediaOptions options = null;

            resource.WriteToXml(o =>
            {
                options = o;
                o.Namespace = "something.xsd";
            });

            resource.Links.ToSubject();
            resource.ProjectValue(x => x.Line1);

            resource.As<IResourceRegistration>().Modify(theConnegGraph, theBehaviorGraph);

            var connegOutput = theBehaviorGraph
                .BehaviorFor<RestController1>(x => x.Find(null))
                .Output;

            var writerNode = connegOutput.Writers.Last().As<MediaWriterNode>();

            // Assert the xml media
            var objectDef = writerNode.As<IContainerModel>().ToObjectDef()
                .FindDependencyDefinitionFor<IMediaWriter<Xml.Address>>();


            var document = objectDef.DependencyFor<IMediaDocument>().ShouldBeOfType<ConfiguredDependency>();
            document.DependencyType.ShouldEqual(typeof (IMediaDocument));
            document.Definition.Type.ShouldEqual(typeof (XmlMediaDocument));
            document.Definition.DependencyFor<XmlMediaOptions>().ShouldBeOfType<ValueDependency>()
                .Value.ShouldBeTheSameAs(options);

            objectDef.DependencyFor<ILinkSource<Xml.Address>>().ShouldBeOfType<ValueDependency>()
                .Value.ShouldBeTheSameAs(resource.Links);


            objectDef.DependencyFor<IProjection<Xml.Address>>().ShouldBeOfType<ValueDependency>()
                .Value.ShouldBeOfType<Projection<Xml.Address>>();
        }

        [Test]
        public void conneg_attachement_policy_finds_and_applies_resource_configuration()
        {
            var registry = new FubuRegistry();

            var typePool = new TypePool();
            typePool.AddAssembly(Assembly.GetExecutingAssembly());

            registry.Policies.Add(x => {
                x.Conneg.ApplyConneg();
            });

            registry.Policies.Add(new ConnegAttachmentPolicy(typePool));
            registry.Services<ResourcesServiceRegistry>();

            registry.Actions.IncludeType<RestController1>();


            var graph = BehaviorGraph.BuildFrom(registry);

            var connegOutput = graph
                .BehaviorFor<RestController1>(x => x.Find(null))
                .Output;

            var writerNode = connegOutput.Writers.Single().As<MediaWriterNode>();

            // Assert the xml media
            var objectDef = writerNode.As<IContainerModel>().ToObjectDef()
                .FindDependencyDefinitionFor <IMediaWriter<Xml.Address>>();


            var document = objectDef.DependencyFor<IMediaDocument>().ShouldBeOfType<ConfiguredDependency>();
            document.DependencyType.ShouldEqual(typeof(IMediaDocument));
            document.Definition.Type.ShouldEqual(typeof(XmlMediaDocument));
            document.Definition.DependencyFor<XmlMediaOptions>().ShouldBeOfType<ValueDependency>()
                .Value.ShouldBeOfType<XmlMediaOptions>().Namespace.ShouldEqual("something.xsd");

            objectDef.DependencyFor<ILinkSource<Xml.Address>>().ShouldBeOfType<ValueDependency>()
                .Value.ShouldNotBeNull();


            objectDef.DependencyFor<IProjection<Xml.Address>>().ShouldBeOfType<ValueDependency>()
                .Value.ShouldBeOfType<Projection<Xml.Address>>();

        }

        public class AddressResource : Resource<Xml.Address>
        {
            public AddressResource()
            {
                WriteToXml(o =>
                {
                    o.Namespace = "something.xsd";
                });

                Links.ToSubject();
                ProjectValue(x => x.Line1);
            }
        }


        public class Input1{}
        public class Input2{}
        public class Input3{}
        public class Input4{}

        public class Output1{}
        public class Output2{}
        public class Output3{}
        public class Output4{}

        public class RestController1
        {
            public Xml.Address Find(Input1 input)
            {
                return new Xml.Address();
            }

            public Xml.Address put_city(Input2 input)
            {
                return new Xml.Address();
            }

            public Xml.Address Method3(Input3 input)
            {
                return new Xml.Address();
            }

            public Output1 Method4(Input1 input)
            {
                return null;
            }

            public Output2 Method5(Input2 input)
            {
                return null;
            }

            [FubuPartial]
            public Output1 NotMe(Input1 input)
            {
                return null;
            }
        }
    }
}