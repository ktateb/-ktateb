using System.Collections.Generic;

namespace Common.Services
{
    public enum ResultStatusCode
    {
        Ok = 200, NotFound = 404, Unauthorized = 401, BadRequest = 400, InternalServerError = 500
    }

    public class ResultService<T>
    {
        public string ErrorField { get; set; }
        public T Result { get; set; }
        public ResultStatusCode Code { get; set; } = ResultStatusCode.Ok;
        public string Messege { get; set; } = "Done";

        public ResultService<T> SetErrorField(string ErrorField)
        {
            this.ErrorField = ErrorField;
            return this;
        }
        public ResultService<T> SetResult(T Result)
        {
            this.Result = Result;
            return this;
        }
        public ResultService<T> SetCode(ResultStatusCode Code)
        {
            this.Code = Code;
            return this;
        }
        public ResultService<T> SetMessege(string Messege)
        {
            this.Messege = Messege;
            return this;
        }
        public static ResultService<T> GetErrorResult() =>
            new() { Code = ResultStatusCode.InternalServerError, Messege = "Some Error Happen" };
    }
}