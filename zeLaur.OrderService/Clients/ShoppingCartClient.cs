using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace zeLaur.OrderService.Clients
{
    public class ShoppingCartClient : IShoppingCartClient
    {
        private readonly HttpClient _client;
        private readonly AppConfig _config;

        public ShoppingCartClient(HttpClient client, AppConfig config)
        {
            _client = client;
            _config = config;
        }

        public async Task<List<CartItem>> GetCartItems(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{id}");

            using (var response = await this._client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CartItem>>(content);
            }
        }
    }
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public int Count { get; set; }
    }
}
