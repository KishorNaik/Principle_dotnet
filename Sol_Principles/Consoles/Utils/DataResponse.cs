using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consoles.Utils;

public sealed class DataResponse<TResponse>
    where TResponse : class
{
    public DataResponse(bool? success, int? statusCode, TResponse? data, string? message)
    {
        this.Success = success;
        this.StatusCode = statusCode;
        this.Data = data;
        this.Message = message;
    }

    public bool? Success { get; }

    public int? StatusCode { get; }

    public string? Message { get; }

    public TResponse? Data { get; }
}

public static class DataResponseFactory
{
    public static DataResponse<TResponse> Response<TResponse>(bool? success, int? statusCode, TResponse? data, string? message)
        where TResponse : class
    {
        return new DataResponse<TResponse>(success, statusCode, data, message);
    }
}