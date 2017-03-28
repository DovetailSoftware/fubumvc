using FubuMVC.Core.UI.Forms;
using FubuMVC.Core.UI.ViewEngine;
using FubuMVC.Core.View;
using HtmlTags.Conventions;

namespace FubuMVC.Core.UI
{
    public class FubuHtmlRegistration : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Policies.Add<HtmlConventionActivation>();
            registry.Services<UIServiceRegistry>();

            registry.ViewFacility(new HtmlDocumentViewFacility());
        }
    }
}