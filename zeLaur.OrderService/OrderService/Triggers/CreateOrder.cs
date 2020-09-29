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
using zeLaur.OrderService.OrderService.OrderOrchestrator;

namespace zeLaur.OrderService.OrderService.Triggers
{
    public static class CreateOrder
    {
        [FunctionName("CreateOrder")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function,  "post", Route = "order/start/{sessionId}")] HttpRequestMessage req,
            Guid sessionId,
            [DurableClient] IDurableClient client )
        {
            if (sessionId == Guid.Empty)
            {
                return (ActionResult) new BadRequestObjectResult("SessionId is required");
            }

            var started = await client.StartNewAsync(nameof(NewOrderOrchestrator), new StartOrderContext
            {
                CorrelationId = Guid.NewGuid().ToString(),
                ShoppingCartId = sessionId.ToString()
            });

            return new AcceptedResult();
        }
    }
}
