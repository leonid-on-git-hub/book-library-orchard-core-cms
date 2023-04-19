using BookLibrary.Storage.Repositories;

var builder = WebApplication.CreateBuilder(args);

Repository.SqlServerConnectionString = builder.Configuration.GetConnectionString("SqlServerConnection").Replace("%CONTENTROOTPATH%", builder.Environment.ContentRootPath);

builder.Services.AddOrchardCms();
var app = builder.Build();

app.UseOrchardCore();

app.Run();
