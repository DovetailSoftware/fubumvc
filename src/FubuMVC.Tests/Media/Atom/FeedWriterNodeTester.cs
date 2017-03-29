using System.Collections;
using System.Collections.Generic;
using FubuCore;
using FubuMVC.Core.Media.Atom;
using FubuMVC.Core.Media.Projections;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Resources.Conneg;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Tests.Media.Atom
{
    [TestFixture]
    public class FeedWriterNodeTester
    {
        public class AddressEnumerable : IEnumerable<Xml.Address>
        {
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<Xml.Address> GetEnumerator()
            {
                yield return new Xml.Address{
                    City = "Austin"
                };
                yield return new Xml.Address{
                    City = "Dallas"
                };
                yield return new Xml.Address{
                    City = "Houston"
                };
            }
        }

        public class AddressValuesEnumerable : IEnumerable<IValues<Xml.Address>>
        {
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<IValues<Xml.Address>> GetEnumerator()
            {
                foreach (var address in new AddressEnumerable())
                {
                    yield return new SimpleValues<Xml.Address>(address);
                }
            }
        }


        [Test]
        public void build_object_def_has_correct_feed_writer_type()
        {
            var objectDef = new FeedWriterNode<Xml.Address>(new Feed<Xml.Address>(), typeof (AddressEnumerable))
                .As<IContainerModel>()
                .ToObjectDef()
                .FindDependencyDefinitionFor<IMediaWriter<IEnumerable<Xml.Address>>>();


            objectDef.Type.ShouldEqual(typeof (FeedWriter<Xml.Address>));
        }

        [Test]
        public void has_a_dependency_for_the_ifeeddefinition()
        {
            var theFeed = new Feed<Xml.Address>();
            var objectDef = new FeedWriterNode<Xml.Address>(theFeed,typeof (AddressEnumerable))
                .As<IContainerModel>()
                .ToObjectDef()
                .FindDependencyDefinitionFor<IMediaWriter<IEnumerable<Xml.Address>>>();


            objectDef.DependencyFor<IFeedDefinition<Xml.Address>>()
                .ShouldBeOfType<ValueDependency>().Value.ShouldBeTheSameAs(theFeed);
        }
    }
}