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
    public static class AddItemToCart
    {
        [FunctionName("AddItemToCart")]
        public static async Task<ActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function,  "post", Route = "cart/{id}/add")] HttpRequestMessage req,
            Guid id,
            [DurableClient] IDurableEntityClient client )
        {
            if (id == Guid.Empty)
            {
                return (ActionResult) new BadRequestObjectResult("Id is required");
            }
                var entityId = new EntityId(nameof(ShoppingCartEntity), id.ToString());

                var data = await req.Content.ReadAsAsync<CartItem>();

                await client.SignalEntityAsync<IShoppingCart>(entityId, proxy => proxy.Add(data));

                return (ActionResult) new AcceptedResult(); // req.CreateResponse(HttpStatusCode.Accepted);

        }
    }
}
