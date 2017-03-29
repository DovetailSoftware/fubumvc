namespace FubuMVC.Core.Media
{
    public class MediaBottleRegistrations : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Services<ResourcesServiceRegistry>();
        }
    }
}