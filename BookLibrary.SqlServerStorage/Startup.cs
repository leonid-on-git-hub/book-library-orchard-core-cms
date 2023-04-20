using BookLibrary.Domain.Repositories;
using BookLibrary.SqlServerStorage.Migrations;
using BookLibrary.Storage.Repositories;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
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
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            Migration();
        }

        #region Private

        private static void Migration()
        {
            var serviceProviderSqlServerDb = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(builder => builder
                    .AddSqlServer()
                    .WithGlobalConnectionString(Repository.SqlServerConnectionString)
                    .ScanIn(typeof(InitialMigration).Assembly).For.Migrations())
                .Configure<RunnerOptions>(opt =>
                {
                    opt.Tags = new[] { "SqlServerDatabase" };
                })
                .BuildServiceProvider(false);

            using var scope = serviceProviderSqlServerDb.CreateScope();
            var runner = serviceProviderSqlServerDb.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        #endregion
    }
}