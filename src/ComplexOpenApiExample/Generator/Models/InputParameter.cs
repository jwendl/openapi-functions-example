using Microsoft.OpenApi.Models;

namespace ComplexOpenApiExample.Generator.Models
{
    public class InputParameter
    {
        public string Name { get; set; }

        public ParameterLocation ParameterLocation { get; set; }

        public string Description { get; set; }
    }
}
