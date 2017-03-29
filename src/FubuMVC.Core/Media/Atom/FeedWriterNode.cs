using System;
using System.Collections.Generic;
using FubuCore.Descriptions;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Resources.Conneg;

namespace FubuMVC.Core.Media.Atom
{
    public class FeedWriterNode<T> : WriterNode
    {
        private readonly IFeedDefinition<T> _feed;

        public FeedWriterNode(IFeedDefinition<T> feed, Type modelType)
        {
            _feed = feed;

            InputType = modelType;
        }

        public IFeedDefinition<T> Feed
        {
            get { return _feed; }
        }

        public Type InputType { get; set; }

        protected override void createDescription(Description description)
        {
            description.Title = "Atom Feed of " + typeof (T).Name;
        }

        public override Type ResourceType
        {
            get { return typeof (IEnumerable<T>); }
        }

        // TODO -- UT this
        public override IEnumerable<string> Mimetypes
        {
            get { yield return _feed.ContentType; }
        }

        protected override ObjectDef toWriterDef()
        {
            var objectDef = new ObjectDef(typeof (FeedWriter<T>));
            objectDef.DependencyByValue(typeof (IFeedDefinition<T>), _feed);

            return objectDef;
        }
    }
}