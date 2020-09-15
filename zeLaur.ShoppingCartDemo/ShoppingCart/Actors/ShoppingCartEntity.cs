using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DurableTask.Core.Stats;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace zeLaur.ShoppingCartDemo.ShoppingCart.Actors
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ShoppingCartEntity
    {

        [JsonProperty("list")]
        private List<CartItem> List { get; set; } = new List<CartItem>();

        public void Add(CartItem item)
        {
            // Get existing
            var existingItem = this.List.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem == null)
            {
                this.List.Add(item);
            }
            else
            {
                existingItem.Count += item.Count;
            }
        }

        public void Remove(CartItem item)
        {
            var existingItem = this.List.FirstOrDefault(i => i.ProductId == item.ProductId);

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
                this.List.Remove(existingItem);
            }
        }

        public ReadOnlyCollection<CartItem> Cart => this.List.AsReadOnly();

         [FunctionName(nameof(ShoppingCartEntity))]
         public static Task Run([EntityTrigger] IDurableEntityContext ctx)
                => ctx.DispatchAsync<ShoppingCartEntity>();
    }

    public class CartItem
    {
        public Guid ProductId { get; set; }
        public int Count { get; set; }
    }
}
