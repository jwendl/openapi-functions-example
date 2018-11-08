using ComplexOpenApiExample.Models;
using ComplexOpenApiExample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace ComplexOpenApiExample.Functions
{
    public class CreateOrder
    {
        private readonly IOrderService orderService;

        public CreateOrder(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [FunctionName("CreateOrder")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var customer = await req.Content.ReadAsAsync<Customer>();
            var result = await orderService.CreateCustomerAsync(customer);

            return new OkObjectResult(result);
        }
    }
}
