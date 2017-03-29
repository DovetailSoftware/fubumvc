using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Media.Atom;
using FubuMVC.Core.Media.Projections;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Tests.Media.Atom
{
    [TestFixture]
    public class FeedSourcesTester
    {
        [Test]
        public void return_values_for_the_main_model()
        {
            var enumerable = new AddressEnumerable();

            var request = new InMemoryFubuRequest();
            request.Set(enumerable);

            var source = new EnumerableFeedSource<Xml.Address>(enumerable);

            source.GetValues().Select(x => x.ValueFor(o => o.City))
                .ShouldHaveTheSameElementsAs("Austin", "Dallas", "Houston");
        }


    }

    public class AddressEnumerable : IEnumerable<Xml.Address>
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Xml.Address> GetEnumerator()
        {
            yield return new Xml.Address(){City = "Austin"};
            yield return new Xml.Address(){City = "Dallas"};
            yield return new Xml.Address(){City = "Houston"};
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
}