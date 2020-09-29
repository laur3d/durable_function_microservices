using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace zeLaur.OrderService.Clients
{
    public interface IShoppingCartClient
    {
        Task<List<CartItem>> GetCartItems(Guid id);
    }
}
