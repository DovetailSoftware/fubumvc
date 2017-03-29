using System.Xml;
using FubuMVC.Core.Media.Projections;

namespace FubuMVC.Core.Media.Xml
{
    public interface IXmlMediaWriter<T>
    {
        XmlDocument WriteValues(IValues<T> values);
        XmlDocument WriteSubject(T subject);
    }
}