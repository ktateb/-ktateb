using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Services.Services
{

    public  class ServiceResult<T>
    {
        public enum ResultCode{
            Done,NotFound,Unauthorized,BadRequist
        }
        public T Result { get  ; set; } 
        public ResultCode Code { get; set; } 
        public string Messege { get; set; }
    }
}