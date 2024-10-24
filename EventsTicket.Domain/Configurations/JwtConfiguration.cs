namespace EventsTicket.Domain.Configurations;

public class JwtConfiguration
{
    public const string SectionName = "JwtConfiguration";
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }
    public short Expires { get; set; }
}
