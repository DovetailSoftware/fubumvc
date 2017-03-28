using System.Collections.Generic;

namespace FubuMVC.Core.View
{
    // TODO -- mark this with an attribute so that it's only used at the app level
    public class CommonViewNamespaces
    {
        private readonly IList<string> _namespaces = new List<string>();
    
        public void AddForType<T>()
        {
            _namespaces.Fill(typeof(T).Namespace);
        }
        
        public void Add(string @namespace)
        {
            _namespaces.Fill(@namespace);
        }

        public IEnumerable<string> Namespaces
        {
            get { return _namespaces; }
        } 
    }
}