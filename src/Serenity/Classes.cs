using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FubuCore.Util;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using OpenQA.Selenium;
using StoryTeller.Engine;

namespace Serenity
{
    public class FubuMvcSystem : BasicSystem
    {
        private readonly SerenityApplications _applications = new SerenityApplications();
        private readonly Func<IWebDriver> _browserBuilder;

        public FubuMvcSystem()
        {
            _browserBuilder = WebDriverSettings.Read().DriverBuilder();
        }

        public SerenityApplications Applications
        {
            get { return _applications; }
        }

        public void AddApplication(IApplicationSource source)
        {
            var settings = ApplicationSettings.ReadByName(source.Name);
            var application = new ApplicationUnderTest(source, settings, _browserBuilder);
            _applications.AddApplication(application);
        }

        public void AddApplication<T>() where T : IApplicationSource, new()
        {
            AddApplication(new T());
        }

        public override void RegisterServices(ITestContext context)
        {
            context.Store(_applications);
            context.Store(_applications.PrimaryApplication());
        }

        public override void SetupEnvironment()
        {
            _applications.Each(x => x.Ping());
        }

        public override void TeardownEnvironment()
        {
            _applications.Each(x => x.Teardown());
        }
    }

    public class SerenityApplications : IEnumerable<IApplicationUnderTest>
    {
        private readonly Cache<string, IApplicationUnderTest> _applications = new Cache<string, IApplicationUnderTest>();
        private IApplicationUnderTest _primary;

        public IEnumerator<IApplicationUnderTest> GetEnumerator()
        {
            return _applications.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddPrimaryApplication(IApplicationUnderTest application)
        {
            AddApplication(application);
            _primary = application;
        }

        public void AddApplication(IApplicationUnderTest application)
        {
            if (!_applications.Any())
            {
                _primary = application;    
            }

            _applications[application.Name] = application;
        }

        public IApplicationUnderTest PrimaryApplication()
        {
            return _primary ?? _applications.GetAll().FirstOrDefault();
        }
    }


    // TODO -- figure out how to ping it
    public interface IApplicationUnderTest
    {
        string Name { get; }
        IUrlRegistry Urls { get; }
        IWebDriver Driver { get; }

        T GetInstance<T>();
        IEnumerable<T> GetAll<T>();

        void Ping();

        void Teardown();
    }


    public class ScreenFixture : Fixture
    {
    }

    public abstract class ScreenFixture<T> : Fixture
    {
    }
}