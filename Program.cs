using Locator.Repositories;
using Locator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<LocatorRepository>();
builder.Services.AddScoped<LocatorService>();

var app = builder.Build();

app.Run();
