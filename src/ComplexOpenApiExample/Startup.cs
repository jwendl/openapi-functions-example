using ComplexOpenApiExample;
using ComplexOpenApiExample.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace ComplexOpenApiExample
{
    internal class Startup
        : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddSingleton<IOrderService, OrderService>();
        }
    }
}
