using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace zeLaur.OrderService.OrderService.OrderOrchestrator.Activities
{
    public class CalculateShippingCostActivity
    {
        [FunctionName(nameof(CalculateShippingCostActivity))]
        public async Task<ShippingCost> Run([ActivityTrigger] ActivityTrigger input)
        {
            // here we would make a call to a dedicated Shipping Cost Calculator that would do this,
            // things like product weight, dimensions from the product catalog might be important apart from the
            // address of the user

            await Task.Delay(3000); // simulate the wait time for a regular http call

            return await Task.FromResult(new ShippingCost
            {
                Cost = 13,
                Currency = "USD"
            });

        }

        public class ActivityTrigger
        {
            public CheckAndReserveItemsActivity.ReservationResult ReservationResult { get; set; }
            public UserData UserData { get; set; }
        }

        public class ShippingCost
        {
            // In real project, I'd suggest that you always use a Money ValueType
            public double Cost { get; set; }
            public string Currency { get; set; }
        }
    }


}
