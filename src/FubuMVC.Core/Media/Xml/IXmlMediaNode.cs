using System.Xml;
using FubuMVC.Core.Media.Projections;

namespace FubuMVC.Core.Media.Xml
{
    public interface IXmlMediaNode : IMediaNode
    {
        XmlElement Element { get; }
        IXmlLinkWriter LinkWriter { get; set; }
    }
}