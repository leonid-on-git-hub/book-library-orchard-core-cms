using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;

namespace BookLibrary.Main
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();
            services.AddCors();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            builder.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

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
