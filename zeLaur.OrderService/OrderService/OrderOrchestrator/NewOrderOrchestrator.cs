using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using zeLaur.OrderService.Clients;
using zeLaur.OrderService.OrderService.OrderOrchestrator.Activities;

namespace zeLaur.OrderService.OrderService.OrderOrchestrator
{
    public class NewOrderOrchestrator
    {
        [FunctionName(nameof(NewOrderOrchestrator))]
        public async Task Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ExecutionContext executionContext)
        {
            var inputData = context.GetInput<StartOrderContext>();
            // step one, get the cart

            context.SetCustomStatus(OrderStatus.GettingProducts.ToString());

            var cartItems = await context.CallActivityWithRetryAsync<List<CartItem>>(nameof(GetShoppingCartActivity),
                new RetryOptions(TimeSpan.FromSeconds(5), 3),
                Guid.Parse(inputData.ShoppingCartId));
            context.SetCustomStatus(OrderStatus.ReservingProducts.ToString());
        }
    }
}
