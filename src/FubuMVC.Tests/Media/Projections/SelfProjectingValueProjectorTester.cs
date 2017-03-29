using System.Linq;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core.Media.Projections;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Tests.Media.Projections
{
    [TestFixture]
    public class SelfProjectingValueProjectorTester
    {
        [Test]
        public void accessors()
        {
            var accessor = ReflectionHelper.GetAccessor<HoldsProjectsItself>(x => x.Itself);
            var projection = new SelfProjectingValueProjector<HoldsProjectsItself, ProjectsItself>(accessor);

            projection.As<IProjection<HoldsProjectsItself>>()
                .Accessors()
                .Single()
                .ShouldEqual(accessor);
        }
    }

    public class ProjectsItself : IProjectMyself
    {
        public void Project(string attributeName, IMediaNode node)
        {
            throw new System.NotImplementedException();
        }
    }

    public class HoldsProjectsItself
    {
        public ProjectsItself Itself { get; set; }
    }
}