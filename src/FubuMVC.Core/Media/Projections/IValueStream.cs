using System.Collections.Generic;

namespace FubuMVC.Core.Media.Projections
{
    public interface IValueStream<T>
    {
        IEnumerable<IValues<T>> Elements { get; }
    }
}