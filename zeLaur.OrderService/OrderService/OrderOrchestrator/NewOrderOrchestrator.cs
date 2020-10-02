using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using zeLaur.OrderService.Clients;
using zeLaur.OrderService.OrderService.OrderOrchestrator.Activities;
using zeLaur.OrderService.OrderService.OrderOrchestrator.PaymentSubOrchestrator;

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
            inputData.UserId = context.NewGuid().ToString();
            // step one, get the cart

            context.SetCustomStatus(OrderStatus.GettingProducts.ToString());

            var cartItems = await context.CallActivityWithRetryAsync<List<CartItem>>(nameof(GetShoppingCartActivity),
                new RetryOptions(TimeSpan.FromSeconds(5), 3),
                Guid.Parse(inputData.ShoppingCartId));

            context.SetCustomStatus(OrderStatus.ReservingProducts.ToString());

            var userData = await context.CallActivityWithRetryAsync<UserData>(nameof(GetUserDetailsActivity),
                new RetryOptions(TimeSpan.FromSeconds(5), 3),
                Guid.Parse(inputData.UserId));

            var wareHouseStocksAvailableAndReserved = await context.CallActivityWithRetryAsync<CheckAndReserveItemsActivity.ReservationResult>(nameof(CheckAndReserveItemsActivity),
                new RetryOptions(TimeSpan.FromSeconds(5), 3),
                cartItems);

            context.SetCustomStatus(OrderStatus.CalculatingShipping.ToString());

            var calculateShippingCost = await context.CallActivityWithRetryAsync<CalculateShippingCostActivity.ShippingCost>(nameof(CalculateShippingCostActivity),
                new RetryOptions(TimeSpan.FromSeconds(5), 3),
                new CalculateShippingCostActivity.ActivityTrigger{
                    ReservationResult = wareHouseStocksAvailableAndReserved,
                    UserData = userData
                });

            context.SetCustomStatus(OrderStatus.CalculatingFinalPrice.ToString());

            var finalCost = await context.CallActivityWithRetryAsync<CalculateFinalCostActivity.FinalCost>(nameof(CalculateFinalCostActivity),
                new RetryOptions(TimeSpan.FromSeconds(5), 3),
                new CalculateFinalCostActivity.ActivityTrigger{
                    ReservationResult = wareHouseStocksAvailableAndReserved,
                    ShippingCost = calculateShippingCost
                });

            context.SetCustomStatus(OrderStatus.AwaitingPayment.ToString());

            // here is a more interesting part
            // let's say we are using an external payment provider that gives uses a webhook

            var paymentSuccess = await context.CallSubOrchestratorAsync<bool>(nameof(PaymentOrchestrator), finalCost);

            context.SetCustomStatus(OrderStatus.SendShippingOrder.ToString());

            // we would have another one here ... but it the same as the the other interaction calls so we skip it :)

            context.SetCustomStatus(OrderStatus.Completed.ToString());

        }
    }
}
