using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace zeLaur.OrderService.OrderService.OrderOrchestrator.Activities
{
    public class GetUserDetailsActivity
    {
        [FunctionName(nameof(GetUserDetailsActivity))]
        public async Task<UserData> Run([ActivityTrigger] Guid userId)
        {
            // here we would also use a client and get the user details from a service


            await Task.Delay(3000); // simulate the wait time for a regular http call

            return await Task.FromResult(new UserData
            {
                Id = userId,
                Email = "Random@random.com",
                Address = "City"
            });
        }
    }

    public class UserData
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
