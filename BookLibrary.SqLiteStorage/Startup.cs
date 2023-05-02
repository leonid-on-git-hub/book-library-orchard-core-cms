using BookLibrary.Domain.Repositories;
using BookLibrary.SqLiteStorage.Migrations;
using BookLibrary.SqLiteStorage.Repositories;
using BookLibrary.Storage.Repositories;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using System.Data;

namespace BookLibrary.SqLiteStorage
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDbConnection, SqliteConnection>();
            services.AddScoped<IBooksRepository, SqLiteBooksRepository>();
            services.AddScoped<IBookTracksRepository, SqLiteBookTracksRepository>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            Migration();
        }

        #region Private

        private static void Migration()
        {
            var serviceProviderSqLiteDb = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(builder => builder
                    .AddSQLite()
                    .WithGlobalConnectionString(Repository.SqLiteConnectionString)
                    .ScanIn(typeof(InitialMigration).Assembly).For.Migrations())
                .Configure<RunnerOptions>(opt =>
                {
                    opt.Tags = new[] { "SqLiteDatabase" };
                })
                .BuildServiceProvider(false);

            using var scope = serviceProviderSqLiteDb.CreateScope();
            var runner = serviceProviderSqLiteDb.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        #endregion
    }
}