using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "cart/{id}")] HttpRequestMessage req,
            Guid id,
            [DurableClient] IDurableClient client,
            ILogger log)
        {
            if (id == Guid.Empty)
            {
                return (ActionResult) new BadRequestObjectResult("The ID is required");
            }

            var entityId = new EntityId(nameof(ShoppingCartEntity), id.ToString());

            var stateResponse = await client.ReadEntityStateAsync<ShoppingCartEntity>(entityId);

            if (!stateResponse.EntityExists)
            {
                return (ActionResult) new NotFoundObjectResult("No cart with this id");
            }

            var response = stateResponse.EntityState.GetCartItems();

            return (ActionResult) new OkObjectResult(response.Result);
        }
    }
}
