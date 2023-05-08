using BookLibrary.Storage.Repositories;
using OrchardCore.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNLogHost();

Repository.SqlServerConnectionString = builder.Configuration.GetConnectionString("SqlServerConnection")?.Replace("%CONTENTROOTPATH%", builder.Environment.ContentRootPath);
Repository.SqLiteConnectionString = builder.Configuration.GetConnectionString("SqLiteConnection")?.Replace("%CONTENTROOTPATH%", builder.Environment.ContentRootPath);

builder.Services.AddOrchardCms();
var app = builder.Build();

app.UseOrchardCore();

app.Run();
