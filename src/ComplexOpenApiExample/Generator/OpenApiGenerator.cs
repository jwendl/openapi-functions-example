using ComplexOpenApiExample.Generator.Models;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComplexOpenApiExample.Generator
{
    public interface IOpenApiGenerator
    {
        OpenApiDocument GenerateDocument(OpenApiInfo openApiInfo, IEnumerable<RestEndpoint> restEndpoints, IEnumerable<ResponseValue> responseValues, IEnumerable<Type> types);
    }

    public class OpenApiGenerator
        : IOpenApiGenerator
    {
        public OpenApiDocument GenerateDocument(OpenApiInfo openApiInfo, IEnumerable<RestEndpoint> restEndpoints, IEnumerable<ResponseValue> responseValues, IEnumerable<Type> types)
        {
            var document = new OpenApiDocument()
            {
                Info = openApiInfo,
                // TODO: auto discover this somehow?
                Servers = new List<OpenApiServer>()
                {
                    new OpenApiServer() { Url = "https://localhost:7071/" },
                },
            };

            var responses = BuildResponses(responseValues);
            var schemas = BuildSchemas(types);
            document.Components = new OpenApiComponents()
            {
                Responses = responses as Dictionary<string, OpenApiResponse>,
                Schemas = schemas as Dictionary<string, OpenApiSchema>,
            };

            document.Paths = BuildPaths(restEndpoints);
            return document;
        }

        private static Dictionary<string, OpenApiSchema> BuildSchemas(IEnumerable<Type> types)
        {
            var typeSchema = new Dictionary<string, OpenApiSchema>();
            foreach (var type in types)
            {
                var properties = new Dictionary<string, OpenApiSchema>();
                foreach (var property in type.GetProperties())
                {
                    properties.Add(property.Name, new OpenApiSchema()
                    {
                        Type = property.DeclaringType.Name,
                        Format = property.DeclaringType.Name,
                    });
                }

                var propertySchema = new OpenApiSchema()
                {
                    Type = "object",
                    Properties = properties,
                };
            }

            return typeSchema;
        }

        private static OpenApiPaths BuildPaths(IEnumerable<RestEndpoint> restEndpoints)
        {
            var dictionary = restEndpoints
                .ToDictionary(re => re.Path, re => new OpenApiPathItem()
                {
                    Parameters = re.InputParameters
                        .Select(ip => new OpenApiParameter()
                        {
                            Name = ip.Name,
                            In = ip.ParameterLocation,
                            Required = true,
                            Schema = new OpenApiSchema()
                            {
                                Type = "string",
                            },
                            Description = ip.Description,
                        }).ToList(),

                    Operations = re.InputOperations
                        .ToDictionary(io => io.OperationType, io => new OpenApiOperation()
                        {
                            Description = io.Description,
                            Responses = io.ResponseValues
                                .ToDictionary(rv => rv.Name, rv => new OpenApiResponse()
                                {
                                    Reference = new OpenApiReference()
                                    {
                                        Id = rv.Id,
                                        Type = ReferenceType.Response,
                                    },
                                }) as OpenApiResponses,
                        }),
                });

            var openApiPaths = new OpenApiPaths();
            foreach (var kvp in dictionary)
            {
                openApiPaths.Add(kvp.Key, kvp.Value);
            }

            return openApiPaths;
        }

        private static Dictionary<string, OpenApiResponse> BuildResponses(IEnumerable<ResponseValue> responseValues)
        {
            return responseValues
                .ToDictionary((rs) => rs.Id, rv => new OpenApiResponse()
                {
                    Description = rv.Description,
                    Content =
                    {
                        [rv.MediaType] = new OpenApiMediaType()
                        {
                            Schema = new OpenApiSchema()
                            {
                                Items = new OpenApiSchema()
                                {
                                    Reference = new OpenApiReference()
                                    {
                                        Id = rv.Name,
                                        Type = ReferenceType.Schema,
                                    },
                                }
                            }
                        }
                    }
                });
        }
    }
}
