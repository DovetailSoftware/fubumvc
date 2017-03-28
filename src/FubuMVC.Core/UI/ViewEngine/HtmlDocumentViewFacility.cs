using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuMVC.Core.Registration;
using FubuMVC.Core.View;

namespace FubuMVC.Core.UI.ViewEngine
{
    public class HtmlDocumentViewFacility : IViewFacility
    {
        public IEnumerable<IViewToken> FindViews(BehaviorGraph graph)
        {
            var types = new TypePool();
            types.AddAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            types.IgnoreExportTypeFailures = true;

            return types.TypesMatching(t => t.IsConcrete() && t.Closes(typeof (FubuHtmlDocument<>)) && !t.Equals(typeof(FubuHtmlDocument<>)))
                .Select(t => new HtmlDocumentViewToken(t));
        }
    }
}