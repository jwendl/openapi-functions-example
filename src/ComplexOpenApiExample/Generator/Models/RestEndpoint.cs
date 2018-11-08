using System.Collections.Generic;

namespace ComplexOpenApiExample.Generator.Models
{
    public class RestEndpoint
    {
        public string Path { get; set; }

        public IEnumerable<InputOperation> InputOperations { get; set; }

        public IEnumerable<ResponseValue> ResponseValues { get; set; }

        public IEnumerable<InputParameter> InputParameters { get; set; }
    }
}
