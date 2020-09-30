using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using zeLaur.OrderService.Clients;

namespace zeLaur.OrderService.OrderService.OrderOrchestrator.PaymentSubOrchestrator.Activities
{
    public class StartPaymentProcessActivity
    {
        private readonly ILogger<StartPaymentProcessActivity> _logger;

        public StartPaymentProcessActivity(ILogger<StartPaymentProcessActivity> logger)
        {
            _logger = logger;
        }

        [FunctionName(nameof(StartPaymentProcessActivity))]
        public async Task Run([ActivityTrigger] ActivityTrigger input)
        {
            _logger.LogCritical($"InstanceID : {input.OrchestratorInstanceId}");

            await Task.Delay(TimeSpan.FromSeconds(3));
        }

        public class ActivityTrigger
        {
            public Guid OrderId { get; set; }
            public List<CartItem> Items { get; set; }
            public double Cost { get; set; }
            public string OrchestratorInstanceId { get; set; }
        }
    }
}
