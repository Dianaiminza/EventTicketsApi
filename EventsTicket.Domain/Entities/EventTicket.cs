using Infrastructure.shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTicket.Domain.Entities;

public class EventTicket : BaseEntity
{
    public long Id { get; set; }
    public string EventName { get; set; }
    public string Venue { get; set; }
    public DateTimeOffset EventDate { get; set; }
    public double Price { get; set; }
    public long AvailableTickets { get; set; }
}