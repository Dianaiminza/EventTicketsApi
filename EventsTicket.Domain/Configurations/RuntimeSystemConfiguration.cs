namespace EventsTicket.Domain.Configurations;

public class RuntimeSystemConfiguration
{
    public const string SectionName = "RuntimeSystemConfiguration";
    public const string PathPrefix = "PathPrefix";
    public bool UseFakePaymentServices { get; set; }
    public static bool ForceHttpsRedirect { get; set; }
    public bool AllowTestEncryptionEndpoints { get; set; }
    public bool EnableElasticSearchApm { get; set; }
    public const string EnableSwaggerEndpointConventions = "EnableSwaggerEndpointConventions";
}
