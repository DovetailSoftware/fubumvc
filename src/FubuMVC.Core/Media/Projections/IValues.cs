using FubuCore.Reflection;

namespace FubuMVC.Core.Media.Projections
{
    public interface IValues<T>
    {
        T Subject { get; }
        object ValueFor(Accessor accessor);
        
    }

}