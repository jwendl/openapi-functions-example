using System;
using System.Net;

namespace ComplexOpenApiExample.Generator.Models
{
    public class ResponseValue
    {
        public string Id
        {
            get
            {
                return $"Name-{Guid.NewGuid()}";
            }
        }

        public string Name { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string MediaType { get; set; }

        public Type ResponseModelType { get; set; }

        public string Description { get; set; }
    }
}
