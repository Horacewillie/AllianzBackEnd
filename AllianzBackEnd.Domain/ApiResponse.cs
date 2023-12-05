using AllianzBackEnd.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AllianzBackEnd.Domain
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }

        public ResultStatus Status { get; set; }

        public string StatusText => Status.ToString();

        public string? Message { get; set; }

        [JsonIgnore]
        public Exception? Exception { get; set; }
        [JsonIgnore]
        public Dictionary<string, string>? ErrorDictionary { get; }

        public ApiResponse()
        {
        }

        public ApiResponse(string message)
            : this()
        {
            Message = message;
        }

        public ApiResponse(Dictionary<string, string> errors,
           string message = "One or more errors occured")
           : this(message)
        {
            ErrorDictionary = errors;
            Status = ResultStatus.Failed;
        }


        public static ApiResponse<T> Error(string message, Exception? ex = null)
        {
            return new ApiResponse<T>
            {
                Message = message,
                Status = ResultStatus.Failed,
                Exception = ex
            };
        }

        public static ApiResponse<T> Ok(T data, string? message = null)
        {
            return new ApiResponse<T>
            {
                Data = data,
                Status = ResultStatus.Complete,
                Message = message
            };
        }
    }



    public static class ApiResponse
    {
        public static ApiResponse<TData> Ok<TData>(TData data, string? message = null)
        {
            return new ApiResponse<TData>
            {
                Data = data,
                Status = ResultStatus.Complete,
                Message = message
            };
        }

        public static ApiResponse<TData> Created<TData>(TData data, string? message = null)
        {
            return new ApiResponse<TData>
            {
                Data = data,
                Status = ResultStatus.Created,
                Message = message
            };
        }

        public static ApiResponse<object> Error<TData>(string message)
        {
            return new ApiResponse<object>
            {
                Message = message,
                Status = ResultStatus.Failed

            };
        }
    }
}
