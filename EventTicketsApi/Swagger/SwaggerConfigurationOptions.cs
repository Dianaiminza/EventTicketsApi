using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace EventTicketsApi.Swagger;

public class SwaggerConfigurationOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SwaggerConfigurationOptions"/> class.
    /// </summary>
    /// <param name="provider"></param>
    public SwaggerConfigurationOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
        }
    }

    /// <inheritdoc />
    public void Configure(string name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static OpenApiInfo CreateVersionInfo(
        ApiVersionDescription description)
    {
        var assemblyDescription =
            typeof(Program).Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
        var info = new OpenApiInfo()
        {
            Title = $"Events Ticket API v{description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
            Description = description.IsDeprecated
                ? $"{assemblyDescription} - DEPRECATED"
                : $"{assemblyDescription}",
            Contact = new OpenApiContact
            {
                Name = "EventsTicket",
                Url = new Uri("https://github.com/Dianaiminza")
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}
