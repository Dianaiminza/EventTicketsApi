using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.shared.CustomExceptions.Models;

public class ValidationError
{
    public ValidationError(string key, string message)
    {
        Key = key;
        Message = message;
    }

    /// <summary>The name of the field that this error relates to.</summary>
    public string Key { get; set; }

    /// <summary>The error message for this validation error.</summary>
    public string Message { get; set; }
}
