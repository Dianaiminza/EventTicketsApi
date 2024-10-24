using Infrastructure.shared.CustomExceptions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.shared.CustomExceptions;

public class ApiException : Exception
{
    public ApiException()
    {
        Errors = Enumerable.Empty<ValidationError>();
    }

    public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) :
      base(message)
    {
        StatusCode = statusCode;
    }

    public ApiException(string message,
      IEnumerable<ValidationError> errors,
      HttpStatusCode statusCode = HttpStatusCode.InternalServerError) :
      base(message)
    {
        StatusCode = statusCode;
        Errors = errors;
    }

    public HttpStatusCode StatusCode { get; set; }

    public IEnumerable<ValidationError> Errors { get; set; }


}
