using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.shared.Wrapper;

public class Result : IResult
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string Message { get; set; } = string.Empty;
    public bool Succeeded { get; set; }

    public static IResult Fail(string message, HttpStatusCode statusCode)
    {
        return new Result { Succeeded = false, StatusCode = statusCode, Message = message };
    }

    public static Task<IResult> FailAsync(string message, HttpStatusCode statusCode)
    {
        return Task.FromResult(Fail(message, statusCode));
    }
}

public sealed class Result<T> : Result, IResult<T>
{
    public T Data { get; set; }

    public new static Result<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T> { Succeeded = false, Message = message, StatusCode = statusCode };
    }

    public static Result<T> Fail(string message, HttpStatusCode statusCode, T data)
    {
        return new Result<T> { Succeeded = false, StatusCode = statusCode, Message = message, Data = data };
    }

    public new static Task<Result<T>> FailAsync(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return Task.FromResult(Fail(message, statusCode));
    }
    public static Task<Result<T>> FailAsync(string message, HttpStatusCode statusCode, T data)
    {
        return Task.FromResult(Fail(message, statusCode, data));
    }
    public static Result<T> Success(string message)
    {
        return new Result<T> { Succeeded = true, Message = message };
    }

    public static Result<T> Success(T data, string message)
    {
        return new Result<T> { Succeeded = true, Data = data, Message = message };
    }

    public static Task<Result<T>> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }

    public static Task<Result<T>> SuccessAsync(T data, string message = "Success")
    {
        return Task.FromResult(Success(data, message));
    }
}