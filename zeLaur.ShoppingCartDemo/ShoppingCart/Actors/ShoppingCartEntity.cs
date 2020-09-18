using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DurableTask.Core.Stats;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace zeLaur.ShoppingCartDemo.ShoppingCart.Actors
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ShoppingCartEntity : IShoppingCart
    {

        [JsonProperty("list")]
        private List<CartItem> list { get; set; } = new List<CartItem>();

        public void Add(CartItem item)
        {
            // Get existing
            var existingItem = this.list.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem == null)
            {
                this.list.Add(item);
            }
            else
            {
                existingItem.Count += item.Count;
            }
        }

        public void Remove(CartItem item)
        {
            var existingItem = this.list.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem == null)
            {
                return;
            }

            if (existingItem.Count > item.Count)
            {
                existingItem.Count -= item.Count;
            }
            else
            {
                this.list.Remove(existingItem);
            }
        }

        public Task<ReadOnlyCollection<CartItem>> GetCartItems()
        {
            return Task.FromResult(this.list.AsReadOnly());
        }

         [FunctionName(nameof(ShoppingCartEntity))]
         public static Task Run([EntityTrigger] IDurableEntityContext ctx)
                => ctx.DispatchAsync<ShoppingCartEntity>();
    }
}
