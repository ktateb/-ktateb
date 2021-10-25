using System.Collections.Generic;

namespace Common.Services
{
    public enum ResultStatusCode
    {
        Ok = 200, NotFound = 404, Unauthorized = 401, BadRequest = 400, InternalServerError =500
    }

    public class ResultService<T>
    {
        public string ErrorField { get; set; }
        public T Result { get; set; }
        public ResultStatusCode Code { get; set; } = ResultStatusCode.Ok;
        public string Messege { get; set; } = "Done";
    }
}