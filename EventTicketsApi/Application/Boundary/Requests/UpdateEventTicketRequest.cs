using System.ComponentModel.DataAnnotations;

namespace EventTicketsApi.Application.Boundary.Requests
{
    public class UpdateEventTicketRequest
    {
        public long Id { get; set; }
        [Required]
        public string EventName { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        public DateTimeOffset EventDate { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public double Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Available tickets cannot be negative.")]
        public long AvailableTickets { get; set; }
    }
}
