using System.ComponentModel.DataAnnotations;

namespace EventTicketsApi.Application.Boundary.Requests
{
    public class CreateEventTicketRequest
    {
        [Required]
        public string EventName { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Available tickets cannot be negative.")]
        public int AvailableTickets { get; set; }
    }
}
