using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using URA.Adapter.Extensions;
using Serilog;
using EventsTicket.Domain.Configurations;
using EventTicketsApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddUraSharedServices(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddSwaggerConfiguration();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfiguration();

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV";
    o.SubstituteApiVersionInUrl = true;
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
});


var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

var swaggerPrefix = builder.Configuration[$"{RuntimeSystemConfiguration.SectionName}:PathPrefix"] ?? string.Empty;

if (!string.IsNullOrEmpty(swaggerPrefix) && swaggerPrefix.StartsWith("/"))
{
    app.UsePathBase(swaggerPrefix);
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"{swaggerPrefix}/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }
});

app.UseMiddleware<ExceptionMiddleware>();


app.UseCors("CorsPolicy");

if (bool.TryParse(builder.Configuration[$"{RuntimeSystemConfiguration.SectionName}:ForceHttpsRedirect"], out var forceHttpsRedirect))
{
    if (forceHttpsRedirect)
    {
        app.UseHttpsRedirection();
    }
}

app.UseAuthorization();


app.MapControllers();
app.MigrateDatabase();



app.Lifetime.ApplicationStarted.Register(OnStarted);
app.Lifetime.ApplicationStopping.Register(OnApplicationShutdown);

app.Run();

void OnStarted()
{
    Console.WriteLine("Application started");
}

void OnApplicationShutdown()
{
    Log.Information("Application shutting down");
    Log.CloseAndFlush();

    // Wait while the data is flushed
    Thread.Sleep(2000);
}
