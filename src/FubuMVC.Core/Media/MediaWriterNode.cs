using System;
using System.Collections.Generic;
using FubuCore.Descriptions;
using FubuMVC.Core.Media.Projections;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Resources.Conneg;

namespace FubuMVC.Core.Media
{
    public class MediaWriterNode : WriterNode
    {
        private readonly IList<string> _mimeTypes = new List<string>();
        private readonly Type _resourceType;

        public MediaWriterNode(Type resourceType)
        {
            _resourceType = resourceType;

            Links = new MediaDependency(typeof (ILinkSource<>), _resourceType);
            Projection = new MediaDependency(typeof (IProjection<>), _resourceType);
            Document = new MediaDependency(typeof (IMediaDocument));
        }

        public MediaDependency Links { private set; get; }
        public MediaDependency Projection { private set; get; }
        public MediaDependency Document { private set; get; }

        protected override void createDescription(Description description)
        {
            description.Title = "Projection Media for " + _resourceType.Name;
        }

        public override Type ResourceType
        {
            get { return _resourceType; }
        }

        public override IEnumerable<string> Mimetypes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override ObjectDef toWriterDef()
        {
            var objectDef = new ObjectDef(typeof (MediaWriter<>).MakeGenericType(_resourceType));

            if (Links.Dependency != null) objectDef.Dependency(Links.Dependency);
            if (Projection.Dependency != null) objectDef.Dependency(Projection.Dependency);
            if (Document.Dependency != null) objectDef.Dependency(Document.Dependency);

            return objectDef;
        }
    }
}