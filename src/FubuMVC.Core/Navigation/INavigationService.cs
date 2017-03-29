using System.Collections.Generic;
using FubuLocalization;

namespace FubuMVC.Core.Navigation
{
    public interface INavigationService
    {
        IEnumerable<MenuItemToken> MenuFor(StringToken key);
    }
}