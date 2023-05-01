using Scaffolding.Service.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Service.Infrastructure
{
    /// <summary>
    /// Class of response object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        public Response() { }

        public Response(bool success, string message)
        {
            Success = success;
            Message = message;

        }
        public T? Item { get; set; }

        public string Message { get; set; } = default!;

        public bool Success { get; set; } = default!;
        public int StatusCode { get; set; }
        
    }
}
