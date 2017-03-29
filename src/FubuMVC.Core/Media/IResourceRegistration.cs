using FubuMVC.Core.Registration;

namespace FubuMVC.Core.Media
{
    public interface IResourceRegistration
    {
        void Modify(ConnegGraph graph, BehaviorGraph behaviorGraph);
    }
}