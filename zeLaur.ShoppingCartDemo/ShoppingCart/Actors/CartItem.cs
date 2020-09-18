using System;

namespace zeLaur.ShoppingCartDemo.ShoppingCart.Actors
{
    public abstract class CartItem
    {
        public Guid ProductId { get; set; }
        public int Count { get; set; }
    }
}