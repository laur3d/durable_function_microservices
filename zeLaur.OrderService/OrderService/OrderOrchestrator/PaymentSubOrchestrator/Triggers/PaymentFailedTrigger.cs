using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace zeLaur.OrderService.OrderService.OrderOrchestrator.PaymentSubOrchestrator.Triggers
{
    public static class PaymentFailedTrigger
    {
        [FunctionName("PaymentFailedTrigger")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "order/{id}/payment-failed")]
            string id,
            [DurableClient] IDurableOrchestrationClient client)
        {

            await client.RaiseEventAsync(id, "payment-request-fail");
            return (IActionResult) new OkObjectResult($"Thank you!");

        }
    }
}
