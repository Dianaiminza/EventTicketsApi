using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.shared.CustomExceptions;

public static class ExceptionExtensions
{
    public static string GetFullMessage(this Exception ex)
    {
        if (ex == null)
        {
            return "Exception message is empty";
        }

        return (ex.Message + " " + ex.InnerException?.GetFullMessage()).TrimEnd();
    }
}
