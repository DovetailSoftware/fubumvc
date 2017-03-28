using System;
using System.Collections.Generic;
using FubuCore;
using FubuCore.Util;
using FubuMVC.Core.View.Model;
using FubuMVC.Core.View.Spark.SparkModel;
using Spark;

namespace FubuMVC.Core.View.Spark
{
    public class SparkEngineSettings
    {
        private CompositeAction<TemplateComposer<ITemplate>> _configurations = new CompositeAction<TemplateComposer<ITemplate>>(); 

        public SparkEngineSettings()
        {
            defaultSearch();
            defaultComposer();
        }

        private void defaultSearch()
        {
            Search = new FileSet { DeepSearch = true };
            Search.AppendInclude("*{0}".ToFormat(Constants.DotSpark));
            Search.AppendInclude("*{0}".ToFormat(Constants.DotShade));
            Search.AppendInclude("bindings.xml");                        
        }

        private void defaultComposer()
        {
            Register(composer => composer
                .AddBinder<ViewDescriptorBinder>()
                .AddBinder<GenericViewModelBinder<ITemplate>>()
                .AddBinder<ViewModelBinder<ITemplate>>()
                .Apply<NamespacePolicy>()
                .Apply<ViewPathPolicy<ITemplate>>());            
        }

        public void Register(Action<TemplateComposer<ITemplate>> alteration)
        {
            _configurations += alteration;
        }

        public void ResetComposerConfiguration()
        {
            _configurations = new CompositeAction<TemplateComposer<ITemplate>>();
        }

        public void Configure(TemplateComposer<ITemplate> composer)
        {
            _configurations.Do(composer);
        }

        /// <summary>
        /// List of namespaces that will be applied as global namespaces to the SparkViewEngine
        /// </summary>
        public readonly IList<string> UseNamespaces = new List<string>();

        /// <summary>
        /// Adds a namespace to UseNamespaces
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UseNamespaceIncludingType<T>()
        {
            UseNamespaces.Add(typeof(T).Name);
        }

        public FileSet Search { get; private set; }
    }
}