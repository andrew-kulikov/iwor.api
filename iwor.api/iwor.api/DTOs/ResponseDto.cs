using Microsoft.AspNetCore.Http;

namespace iwor.api.DTOs
{
    public class ResponseDto<T>
    {
        public string Status { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }

        public static ResponseDto<T> Ok()
        {
            return new ResponseDto<T>
            {
                Status = "ok",
                Code = StatusCodes.Status200OK,
                Result = default
            };
        }
        public static ResponseDto<T> Ok(T result)
        {
            return new ResponseDto<T>
            {
                Status = "ok",
                Code = StatusCodes.Status200OK,
                Result = result
            };
        }

        public static ResponseDto<T> BadRequest(string message)
        {
            return new ResponseDto<T>
            {
                Status = "error",
                Code = StatusCodes.Status400BadRequest,
                Message = message,
                Result = default
            };
        }

        public static ResponseDto<T> NotFound(string message)
        {
            return new ResponseDto<T>
            {
                Status = "error",
                Code = StatusCodes.Status404NotFound,
                Message = message,
                Result = default
            };
        }
    }
}