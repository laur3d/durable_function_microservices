using System;
using System.IO;
using System.Net;
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
using Newtonsoft.Json.Linq;
using zeLaur.ShoppingCartDemo.ShoppingCart.Actors;

namespace zeLaur.ShoppingCartDemo.ShoppingCart.Triggers
{
    public static class GetCartForSession
    {
        [FunctionName("GetCartForSession")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestMessage req,
            [DurableClient] IDurableClient client,
            ILogger log)
        {
            var entityId = new EntityId(nameof(ShoppingCartEntity),"61283F9B-B3F6-4305-AED4-31573B10CF4A");
            var stateResponse = await client.ReadEntityStateAsync<ShoppingCartEntity>(entityId);

            var response = stateResponse.EntityState.Cart;

                // return req.CreateResponse( stateResponse.EntityState.Cart);
                return (ActionResult) new OkObjectResult(response);
        }
    }
}
