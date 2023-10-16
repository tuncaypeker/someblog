using System;
using System.Net;

namespace SomeBlog.Integration.Pexels.Models
{
    public class ErrorResponse : Exception
    {
        public HttpStatusCode statusCode { get; set; }

        public ErrorResponse() { }

        public ErrorResponse(HttpStatusCode statusCode, string message) : base(message)
        {
            this.statusCode = statusCode;
        }
    }
}
