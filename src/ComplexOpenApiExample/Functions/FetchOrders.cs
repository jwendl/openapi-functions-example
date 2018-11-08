using ComplexOpenApiExample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace ComplexOpenApiExample.Functions
{
    public class FetchOrders
    {
        private readonly IOrderService orderService;

        public FetchOrders(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [FunctionName("FetchOrders")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var customers = await orderService.FetchCustomersAsync();

            return new OkObjectResult(customers);
        }
    }
}
