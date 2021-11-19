using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DotnetMicroserviceArchitecture.Core.Dtos
{
    public class Response<T>
    {
        #region Properties

        public T Data { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }

        [JsonIgnore]
        public bool IsSuccess { get; set; }

        #endregion

        #region Success Static Factory Methods

        public static Response<T> Success(T data, int statusCode) => new Response<T> { Data = data, StatusCode = statusCode, IsSuccess = true };
        public static Response<T> Success(int statusCode) => new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccess = true };

        #endregion

        #region Fail Static Factory Methods

        public static Response<T> Fail(List<string> errors, int statusCode) => new Response<T> { Errors = errors, StatusCode = statusCode, IsSuccess = false };
        public static Response<T> Fail(string error, int statusCode) => new Response<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccess = false };

        #endregion
    }
}
