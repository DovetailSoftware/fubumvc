using System.Collections.Generic;
using FubuMVC.Core.Media.Projections;
using FubuMVC.Core.Urls;

namespace FubuMVC.Core.Media
{
    public interface ILinkSource<T>
    {
        IEnumerable<Link> LinksFor(IValues<T> target, IUrlRegistry urls);
    }
}