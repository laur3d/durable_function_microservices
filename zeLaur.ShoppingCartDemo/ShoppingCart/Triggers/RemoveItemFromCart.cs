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
using zeLaur.ShoppingCartDemo.ShoppingCart.Actors;

namespace zeLaur.ShoppingCartDemo.ShoppingCart.Triggers
{
    public static class RemoveItemFromCart
    {
        [FunctionName("RemoveItemFromCart")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "cart/{id}/remove")] HttpRequestMessage req,
            Guid id,
            [DurableClient] IDurableClient client,  ILogger log)
        {

            if (id == Guid.Empty)
            {
                return (ActionResult) new BadRequestObjectResult("Id is required");
            }
                var entityId = new EntityId(nameof(ShoppingCartEntity),id.ToString());

                var data = await req.Content.ReadAsAsync<CartItem>();

                await client.SignalEntityAsync<IShoppingCart>(entityId, proxy => proxy.Remove(data));

                return (ActionResult) new AcceptedResult();
        }
    }
}
