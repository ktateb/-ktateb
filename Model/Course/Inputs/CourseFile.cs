using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Model.Course.Inputs
{
    public class CourseFile
    {
        public IFormFile File { get; set; }
    }
}