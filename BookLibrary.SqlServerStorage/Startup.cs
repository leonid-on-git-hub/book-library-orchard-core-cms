using BookLibrary.Domain.Repositories;
using BookLibrary.SqlServerStorage.Migrations;
using BookLibrary.Storage.Repositories;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using System.Data;
using System.Data.SqlClient;

namespace BookLibrary.SqlServerStorage
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDbConnection, SqlConnection>();
            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped<IBookTracksRepository, BookTracksRepository>();

            services.AddFluentMigratorCore()
                .ConfigureRunner(
                    builder => builder
                        .AddSqlServer()
                        .WithGlobalConnectionString(Repository.SqlServerConnectionString)
                        .ScanIn(typeof(InitialMigration).Assembly).For.Migrations());
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}