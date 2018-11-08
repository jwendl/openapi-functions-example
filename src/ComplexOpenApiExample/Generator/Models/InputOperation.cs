using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace ComplexOpenApiExample.Generator.Models
{
    public class InputOperation
    {
        public OperationType OperationType { get; set; }

        public string Description { get; set; }

        public IEnumerable<ResponseValue> ResponseValues { get; set; }
    }
}
