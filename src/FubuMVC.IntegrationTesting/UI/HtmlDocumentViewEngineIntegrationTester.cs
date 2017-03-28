using System.Linq;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.UI;
using FubuMVC.Core.UI.ViewEngine;
using FubuMVC.Katana;
using FubuMVC.StructureMap;
using FubuTestingSupport;
using NUnit.Framework;
using StructureMap;

namespace FubuMVC.IntegrationTesting.UI
{
    [TestFixture]
    public class HtmlDocumentViewEngineIntegrationTester
    {
        [Test]
        public void can_render_a_simple_html_document_that_is_attached_via_view_conventions()
        {
            using (var server = FubuApplication.DefaultPolicies().StructureMap(new Container()).RunEmbedded())
            {
                server.Endpoints.Get<DocEndpoint>(x => x.get_document())
                    .ReadAsText().ShouldContain("<h1>Name = Shiner</h1>");
            }


        }

        [Test]
        public void the_view_engine_can_find_documents()
        {
            var graph = new BehaviorGraph();

            var engine = new HtmlDocumentViewFacility();

            engine.FindViews(graph).Any(x => x.ViewType == typeof(DocViewModelDocument))
                .ShouldBeTrue();
        }

        [Test]
        public void the_view_engine_does_not_include_itself_with_no_closing_type()
        {
            var graph = new BehaviorGraph();

            var engine = new HtmlDocumentViewFacility();

            engine.FindViews(graph).Any(x => x.ViewType == typeof(FubuHtmlDocument<>))
                .ShouldBeFalse();
        }

        [Test]
        public void view_token()
        {
            var token = new HtmlDocumentViewToken(typeof (DocViewModelDocument));
            token.ViewModel.ShouldEqual(typeof (DocViewModel));
            token.ViewType.ShouldEqual(typeof (DocViewModelDocument));
            token.Name().ShouldEqual("DocViewModelDocument");
            token.Namespace.ShouldEqual(GetType().Namespace);
        }

        [Test]
        public void view_token_creates_object_def_for_view_factory()
        {
            var token = new HtmlDocumentViewToken(typeof(DocViewModelDocument));
            token.ToViewFactoryObjectDef().Type.ShouldEqual(typeof (HtmlDocumentViewFactory<DocViewModelDocument>));
        }
    }

    public class DocEndpoint
    {
        public DocViewModel get_document()
        {
            return new DocViewModel {Name = "Shiner"};
        }
    }

    public class DocViewModelDocument : FubuHtmlDocument<DocViewModel>
    {
        public DocViewModelDocument(IServiceLocator services, IFubuRequest request) : base(services, request)
        {
            Add("h1").Text("Name = " + Model.Name);
        }
    }

    public class DocViewModel
    {
        public string Name { get; set; }
    }
}