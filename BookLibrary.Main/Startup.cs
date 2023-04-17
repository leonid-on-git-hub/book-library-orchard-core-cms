using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace BookLibrary.Main
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "Home",
                areaName: "BookLibrary.Main",
                pattern: "",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapAreaControllerRoute(
                name: "About",
                areaName: "BookLibrary.Main",
                pattern: "/About",
                defaults: new { controller = "Home", action = "About" }
            );
            routes.MapAreaControllerRoute(
                name: "Contact",
                areaName: "BookLibrary.Main",
                pattern: "/Contact",
                defaults: new { controller = "Home", action = "Contact" }
            );
        }
    }
}
