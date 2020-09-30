using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using zeLaur.OrderService.Clients;

namespace zeLaur.OrderService.OrderService.OrderOrchestrator.Activities
{
    public class CheckAndReserveItemsActivity
    {
        [FunctionName(nameof(CheckAndReserveItemsActivity))]
        public async Task<ReservationResult> Run([ActivityTrigger] List<CartItem> items)
        {
            // we would call the warehouse service / endpoint here

            await Task.Delay(3000); // simulate the wait time for a regular http call

            return await Task.FromResult(new ReservationResult
            {
                    Id = Guid.NewGuid(),
                    SuccessfullyReserved = new Dictionary<Guid, int>(items.Select(i => new KeyValuePair<Guid, int>(i.ProductId, i.Count))),
                    NotSuccessfullyReserved = new Dictionary<Guid, int>()
            });
        }

        public class ReservationResult
        {
            public Guid Id { get; set; }
            public Dictionary<Guid, int> SuccessfullyReserved { get; set; }
            public Dictionary<Guid, int> NotSuccessfullyReserved { get; set; }

        }
    }
}
