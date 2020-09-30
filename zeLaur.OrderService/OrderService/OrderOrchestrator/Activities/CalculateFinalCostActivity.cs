using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace zeLaur.OrderService.OrderService.OrderOrchestrator.Activities
{
    public class CalculateFinalCostActivity
    {
        [FunctionName(nameof(CalculateFinalCostActivity))]
        public async Task<FinalCost> Run([ActivityTrigger] ActivityTrigger input)
        {
            // this could be done right here, or you could also have a dedicated microservice that also tracks deals and coupons and stuff

            await Task.Delay(3000); // simulate the wait time for a regular http call

            return await Task.FromResult(new FinalCost
            {
                Cost = 399.9,
                Currency = "USD"
            });
        }

        public struct FinalCost
        {
            public double Cost { get; set; }
            public string Currency { get; set; }
        }

        public class ActivityTrigger
        {
            public CheckAndReserveItemsActivity.ReservationResult ReservationResult { get; set; }
            public CalculateShippingCostActivity.ShippingCost ShippingCost { get; set; }
        }
    }
}
