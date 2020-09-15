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
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestMessage req,
            [DurableClient] IDurableEntityClient client )
        {
            // log.LogInformation("C# HTTP trigger function processed a request.");
            //
            // string name = req.Query["name"];
            //
            // string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            // name = name ?? data?.name;
            //
            // return name != null
            //     ? (ActionResult) new OkObjectResult($"Hello, {name}")
            //     : new BadRequestObjectResult("Please pass a name on the query string or in the request body");

                var entityId = new EntityId(nameof(ShoppingCartEntity),"61283F9B-B3F6-4305-AED4-31573B10CF4A");

                var cartItem = new CartItem
                {
                    ProductId = Guid.Parse("5D257F47-B4D2-4EF6-8E0F-16B8CA59281D"),
                    Count = 1
                };

                await client.SignalEntityAsync(entityId, "Add", cartItem);



                return req.CreateResponse(HttpStatusCode.Accepted);

        }
    }
}
