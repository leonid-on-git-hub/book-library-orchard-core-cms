using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace BookLibrary.MainModule
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
                areaName: "BookLibrary.MainModule",
                pattern: "",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapAreaControllerRoute(
                name: "About",
                areaName: "BookLibrary.MainModule",
                pattern: "/About",
                defaults: new { controller = "Home", action = "About" }
            );
            routes.MapAreaControllerRoute(
                name: "Contact",
                areaName: "BookLibrary.MainModule",
                pattern: "/Contact",
                defaults: new { controller = "Home", action = "Contact" }
            );
        }
    }
}
