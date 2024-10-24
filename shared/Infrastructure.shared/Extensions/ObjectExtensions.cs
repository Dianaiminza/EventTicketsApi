using FastDeepCloner;
using Infrastructure.shared.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.shared.Extensions;

public static class ObjectExtensions
{
    public static T EnsureExists<T>(this T obj, string errorMessage = "Entity not found", HttpStatusCode statusCode = HttpStatusCode.NotFound)
    {
        if (obj == null)
        {
            throw new ApiException(errorMessage, statusCode);
        }

        return obj;
    }

    public static async Task<T> EnsureExistsAsync<T>(this Task<T> task, string errorMessage = "Entity not found", HttpStatusCode statusCode = HttpStatusCode.NotFound)
    {
        T val = await task.ConfigureAwait(continueOnCapturedContext: false);
        if (val != null)
        {
            return val;
        }

        throw new ApiException(errorMessage, statusCode);
    }

    public static object DeepCopy(this object obj, CloneLevel level)
    {
        return obj.Clone(new FastDeepClonerSettings
        {
            CloneLevel = level
        });
    }

    public static T DeepCopy<T>(this T original, CloneLevel level = CloneLevel.Hierarki)
    {
        return (T)((object)original).DeepCopy(level);
    }
}
