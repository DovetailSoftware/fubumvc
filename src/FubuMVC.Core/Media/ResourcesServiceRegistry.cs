using FubuMVC.Core.Media.Projections;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Runtime.Formatters;

namespace FubuMVC.Core.Media
{
    public class ResourcesServiceRegistry : ServiceRegistry
    {
        public ResourcesServiceRegistry()
        {
            AddService<IFormatter, JsonFormatter>();
            AddService<IFormatter, XmlFormatter>();

            SetServiceIfNone(typeof(IValues<>), typeof(SimpleValues<>));
            SetServiceIfNone(typeof(IValueSource<>), typeof(ValueSource<>));

            SetServiceIfNone<IProjectionRunner, ProjectionRunner>();
            SetServiceIfNone(typeof(IProjectionRunner<>), typeof(ProjectionRunner<>)); 
            SetServiceIfNone<IProjectionRunner, ProjectionRunner>();
            SetServiceIfNone(typeof(IProjectionRunner<>), typeof(ProjectionRunner<>));

        }
    }
}