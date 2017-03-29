using System;
using FubuMVC.Core.Registration.ObjectGraph;

namespace FubuMVC.Core.Media
{
    public interface IMediaReaderNode
    {
        Type InputType { get; }
        ObjectDef ToObjectDef();
    }
}