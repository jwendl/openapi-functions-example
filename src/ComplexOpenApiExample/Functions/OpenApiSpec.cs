using ComplexOpenApiExample.Generator;
using ComplexOpenApiExample.Generator.Models;
using ComplexOpenApiExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ComplexOpenApiExample.Functions
{
    public static class OpenApiSpec
    {
        [FunctionName("GenerateMetadata")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "openapi")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://raw.githubusercontent.com/OAI/OpenAPI-Specification/")
            };

            //var openApiDocument = GenerateOpenApiDocument();
            var openApiInfo = new OpenApiInfo()
            {
                Title = "ComplexOpenApiExample",
                Description = "A complex open api example",
                Version = "1.0.0",
            };

            var restEndpoints = new List<RestEndpoint>()
            {
                new RestEndpoint()
                {
                    Path = "/FetchOrders",
                    InputOperations = new List<InputOperation>()
                    {
                        new InputOperation()
                        {
                            OperationType = OperationType.Get,
                            Description = "Get an order",
                            ResponseValues = new List<ResponseValue>()
                            {
                                new ResponseValue()
                                {
                                    Name = "200",
                                    Description = "A good response",
                                    MediaType = "application/json",
                                    ResponseModelType = typeof(Order),
                                    StatusCode = HttpStatusCode.OK,
                                }
                            }
                        }
                    },
                    InputParameters = new List<InputParameter>()
                    {
                        new InputParameter()
                        {
                            Name = "QueryString",
                            Description = "A querystring",
                            ParameterLocation = ParameterLocation.Query,
                        }
                    },
                },
            };

            var responseValues = restEndpoints.SelectMany(re => re.InputOperations)
                .SelectMany(io => io.ResponseValues);

            var types = responseValues
                .Select(rv => rv.ResponseModelType);

            var openApiGenerator = new OpenApiGenerator();
            var openApiDocument = openApiGenerator.GenerateDocument(openApiInfo, restEndpoints, responseValues, types);
            var openApiString = openApiDocument.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);

            return new OkObjectResult(openApiString);
        }
    }
}
