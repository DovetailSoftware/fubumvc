using FubuCore;
using FubuMVC.Core.Navigation;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Tests.Navigation
{
    [TestFixture]
    public class AddAfterTester
    {
        [Test]
        public void format_description()
        {
            var key = new NavigationKey("something");
            new AddAfter().FormatDescription("parent", key)
                .ShouldEqual("Insert '{0}' after 'parent'".ToFormat(key.ToLocalizationKey()));
        }

        [Test]
        public void apply()
        {
            var dependency = MenuNode.Node("something");
            dependency.AddAfter(MenuNode.Node("third"));
            var node = MenuNode.Node("else");

            new AddAfter().Apply(dependency, node);

            dependency.Next.ShouldBeTheSameAs(node);
        }
    }
}