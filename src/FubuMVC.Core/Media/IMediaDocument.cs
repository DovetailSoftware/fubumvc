using System.Collections.Generic;
using FubuMVC.Core.Media.Projections;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Core.Media
{
    public interface IMediaDocument
    {
        IMediaNode Root { get; }

        IEnumerable<string> Mimetypes { get; }
        void Write(IOutputWriter writer, string mimeType);
    }
}