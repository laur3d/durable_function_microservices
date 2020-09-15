using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace zeLaur.ShoppingCartDemo.ShoppingCart.Actors
{
    public interface IShoppingCart
    {
        void Add(CartItem item);
        void Remove(CartItem item);
        Task<ReadOnlyCollection<CartItem>> GetCartItems();
    }
}
