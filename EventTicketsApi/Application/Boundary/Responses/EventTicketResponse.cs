namespace EventTicketsApi.Application.Boundary.Responses;

public class EventTicketResponse
{
    public long Id { get; set; }
    public string EventName { get; set; }
    public string Venue { get; set; }
    public DateTimeOffset EventDate { get; set; }
    public decimal Price { get; set; }
    public long AvailableTickets { get; set; }
}
