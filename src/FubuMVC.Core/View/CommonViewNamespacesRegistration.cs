namespace FubuMVC.Core.View
{
    public class CommonViewNamespacesRegistration : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Configure(graph => {
                graph.Services.AddService(graph.Settings.Get<CommonViewNamespaces>());
            });
        }
    }
}