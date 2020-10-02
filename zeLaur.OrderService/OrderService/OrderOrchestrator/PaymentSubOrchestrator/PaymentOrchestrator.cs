using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using zeLaur.OrderService.OrderService.OrderOrchestrator.Activities;
using zeLaur.OrderService.OrderService.OrderOrchestrator.PaymentSubOrchestrator.Activities;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

namespace zeLaur.OrderService.OrderService.OrderOrchestrator.PaymentSubOrchestrator
{
    public class PaymentOrchestrator
    {
        [FunctionName(nameof(PaymentOrchestrator))]
        public async Task<bool> Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ExecutionContext executionContext)
        {
            var input = context.GetInput<CalculateFinalCostActivity.FinalCost>();

            // here we need to call our payment provider

            var timeout = TimeSpan.FromSeconds(60); // here we should have a bigger time window... but demo..
            var deadline = context.CurrentUtcDateTime.Add(timeout);

            using var cts = new CancellationTokenSource();

            var timeoutTask = context.CreateTimer(deadline, cts.Token);
            var paymentSuccess = context.WaitForExternalEvent($"payment-request-success");
            var paymentFail = context.WaitForExternalEvent($"payment-request-fail");

            // send the request for payment
            await context.CallActivityAsync(nameof(StartPaymentProcessActivity),
                new StartPaymentProcessActivity.ActivityTrigger
                {
                    Cost = input.Cost,
                    OrchestratorInstanceId = context.InstanceId,
                });

            // wait for the winner - payed, not payed or timed out
            var winner = await Task.WhenAny(timeoutTask, paymentSuccess, paymentFail);

            return winner == paymentSuccess;
        }
    }
}
