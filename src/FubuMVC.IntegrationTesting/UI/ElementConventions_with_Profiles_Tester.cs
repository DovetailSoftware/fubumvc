﻿using System;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.UI;
using FubuMVC.Core.View;
using FubuMVC.Katana;
using FubuMVC.StructureMap;
using FubuMVC.Tests.UI;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;
using StructureMap;

namespace FubuMVC.IntegrationTesting.UI
{
    [TestFixture]
    public class ElementConventions_with_Profiles_Tester
    {
        public class TestRegistry : FubuRegistry
        {
            public TestRegistry()
            {
                Import<ProfiledHtmlConventions>();
                Actions.IncludeType<ProfiledEndpoint>();

                AlterSettings<ViewEngines>(x =>
                {
                    x.IfTheViewMatches(v => v.Name().Contains("Profile")).SetTagProfileTo("foo");
                });
            }
        }



        [Test]
        public void get_profiled_display()
        {
            ProfiledViewModelDocument.Builder = doc => {
                doc.Model.Name = "Jeremy";
                return doc.DisplayFor(x => x.Name);
            };

            using (var server = FubuApplication.For<TestRegistry>().StructureMap(new Container()).RunEmbedded())
            {
                server.Endpoints.Get<ProfiledEndpoint>(x => x.get_profiled_page()).ReadAsText()
                    .ShouldContain("<div class=\"foo\">Jeremy</div>");
            }
        }
    }

    public class ProfiledEndpoint
    {
        public ProfiledViewModel get_profiled_page()
        {
            return new ProfiledViewModel();
        }
    }

    public class ProfiledViewModel : ConventionTarget
    {
        
    }

    public class ProfiledViewModelDocument : FubuHtmlDocument<ProfiledViewModel>
    {
        public static Func<ProfiledViewModelDocument, HtmlTag> Builder = x => new HtmlTag("div").Text("Nothing");

        public ProfiledViewModelDocument(IServiceLocator services, IFubuRequest request) : base(services, request)
        {
            HtmlTag tag = Builder(this);
            Add(tag);
        }
    }

    public class ProfiledHtmlConventions : HtmlConventionRegistry
    {
        public ProfiledHtmlConventions()
        {
            Profile("foo", profile => {
                profile.Displays.Always.BuildBy(request => {
                    return new HtmlTag("div").Text(request.StringValue()).AddClass("foo");
                });
            });
        }
    }

    
}