using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Media.Projections;

namespace FubuMVC.Core.Media.Atom
{
    public class EnumerableFeedSource<T> : IFeedSource<T>
    {
        private readonly IEnumerable<T> _enumerable;

        public EnumerableFeedSource(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }

        public IEnumerable<IValues<T>> GetValues()
        {
            return _enumerable.Select(x => new SimpleValues<T>(x));
        }
    }
}