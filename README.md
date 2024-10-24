# Event Ticket API

A simple RESTful API for managing event tickets built with C# and .NET 8. This API allows users to create, read, update, and delete event tickets, and includes features such as authentication, validation, and pagination.

## Features

- **CRUD Operations**: Create, Read, Update, and Delete event tickets.
- **Authentication**: Secure endpoints with JWT (JSON Web Tokens).
- **Validation**: Input validation using data annotations.
- **Pagination**: Retrieve tickets with pagination support.

## Technologies Used

- C# .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT for authentication

## Getting Started

### Prerequisites

- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- A SQL Server instance

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/Dianaiminza/EventTicketsApi.git
   cd EventTicketsApi
2. Restore the dependencies:

   ```bash
   dotnet restore
3. Update the connection string in appsettings.json:

    ```json
    "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=EventTicketDb;User Id=your_user;Password=your_password;"
   }
4. Run the migrations to set up the database:
   ```bash
     dotnet ef migrations add InitialCreate
     dotnet ef database update
5. Run the application:
    ```bash
    dotnet run

## API Endpoints

# Event Tickets
POST /api/v1/eventtickets: Create a new ticket
     Body:
       ```json
           
          { "eventName": "Event Name", "venue": "Venue", "eventDate": "YYYY-MM-DD", "price": 10.00, "availableTickets": 100 }

GET /api/v1/eventtickets: Get all tickets 

GET /api/v1/eventtickets/{id}: Get ticket by ID

DELETE /api/v1/eventtickets/delete: Delete a ticket by ID

PUT /api/v1/eventtickets/{id}: Update an existing ticket
   Body: 
   ```json
       { "eventName": "Event Name", "venue": "Venue", "eventDate": "YYYY-MM-DD", "price": 10.00, "availableTickets": 100 }
   



