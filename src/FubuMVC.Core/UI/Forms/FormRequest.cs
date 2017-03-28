using FubuCore;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Querying;
using HtmlTags.Conventions;

namespace FubuMVC.Core.UI.Forms
{
    public class FormRequest : TagRequest, IServiceLocatorAware 
    {
        private readonly ChainSearch _search;
        private readonly object _input;
        private IServiceLocator _services;

        public FormRequest(ChainSearch search, object input)
        {
            _search = search;
            _input = input;
        }

        public ChainSearch Search
        {
            get { return _search; }
        }

        public object Input
        {
            get { return _input; }
        }

        public string Url { get; set; }
        public BehaviorChain Chain { get; set; }  

        public override object ToToken()
        {
            return new FormRequest(_search, _input);
        }

        public void Attach(IServiceLocator locator)
        {
            _services = locator;
            var resolver = locator.GetInstance<IChainResolver>();
            var currentHttpRequest = locator.GetInstance<ICurrentHttpRequest>();
            Chain = resolver.Find(Search);

            if (Chain == null)
            {
                throw new FubuException(333, "No chain matches this search:  " + Search.ToString());
            }

            if (Chain.Route == null)
            {
                throw new FubuException(334, "Cannot post to this endpoint because there is no route");
            }

            Url = currentHttpRequest.ToFullUrl(Chain.Route.CreateUrlFromInput(_input));
        }

        public IServiceLocator Services
        {
            get { return _services; }
        }

        protected bool Equals(FormRequest other)
        {
            return Equals(_search, other._search) && Equals(_input, other._input);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FormRequest) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_search != null ? _search.GetHashCode() : 0)*397) ^ (_input != null ? _input.GetHashCode() : 0);
            }
        }
    }
}