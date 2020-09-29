using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using zeLaur.OrderService.Clients;

namespace zeLaur.OrderService.OrderService.OrderOrchestrator.Activities
{
    public class GetShoppingCartActivity
    {
        private readonly IShoppingCartClient _client;

        public GetShoppingCartActivity(IShoppingCartClient client)
        {
            _client = client;
        }
        [FunctionName(nameof(GetShoppingCartActivity))]
        public async Task<List<CartItem>> Run(
            [ActivityTrigger] Guid id)
        {
            try
            {
               return await this._client.GetCartItems(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
