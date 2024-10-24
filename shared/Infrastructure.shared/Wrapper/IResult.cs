using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.shared.Wrapper;

public interface IResult
{
    string Message { get; set; }
    bool Succeeded { get; set; }
    HttpStatusCode StatusCode { get; set; }
}

public interface IResult<out T> : IResult
{
    T Data { get; }
}