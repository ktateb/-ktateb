using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Services.Services
{
    public enum ResultStatusCode
    {
        Ok = 1, NotFound, Unauthorized, BadRequist
    }

    public class ResultService<T>
    {
        public T Result { get; set; }
        public ResultStatusCode Code { get; set; } = ResultStatusCode.Ok;
        public string Messege { get; set; } = "Done";
    }
}