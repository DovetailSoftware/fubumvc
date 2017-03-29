using FubuLocalization;

namespace FubuMVC.Core.Navigation
{
    public class NavigationKey : StringToken
    {
        public NavigationKey(string text) : base(text, text, namespaceByType:true)
        {
        }

        public NavigationKey(string key, string defaultText) : base(key, defaultText, namespaceByType:true)
        {
            
        }
    }
}