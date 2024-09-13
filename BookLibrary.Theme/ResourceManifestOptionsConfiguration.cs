using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace BookLibrary.Theme
{
    public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
    {
        private static readonly ResourceManifest _manifest;

        static ResourceManagementOptionsConfiguration()
        {
            _manifest = new ResourceManifest();

            _manifest
                .DefineStyle("Theme-bootstrap-oc")
                .SetUrl("~/BookLibrary.Theme/css/bootstrap-oc.min.css", "~/BookLibrary.Theme/css/bootstrap-oc.css")
                .SetVersion("1.0.0");
        }

        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(_manifest);
        }
    }
}
