using System.Collections.Generic;
using FubuMVC.Core.Media.Projections;

namespace FubuMVC.Core.Media.Atom
{
    public interface IFeedSource<T>
    {
        IEnumerable<IValues<T>> GetValues();
    }
}