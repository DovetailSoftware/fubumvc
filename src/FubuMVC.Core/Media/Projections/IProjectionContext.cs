using FubuCore.Formatting;
using FubuMVC.Core.Urls;

namespace FubuMVC.Core.Media.Projections
{
    public interface IProjectionContext<T> : IValues<T>
    {
        TService Service<TService>();
        IUrlRegistry Urls { get; }
        IDisplayFormatter Formatter { get; }

        IProjectionContext<TChild> ContextFor<TChild>(TChild child);
    }
}