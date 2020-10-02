using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace zeLaur.OrderService.OrderService.Triggers
{
    public static class GetOrderStatus
    {
        [FunctionName("GetOrderStatus")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = "order/{id}/status")] HttpRequestMessage req,
            [DurableClient] IDurableClient client,
            string id)
        {
            var status = await client.GetStatusAsync(id);

            return (IActionResult) new OkObjectResult(status.CustomStatus);

        }
    }
}
