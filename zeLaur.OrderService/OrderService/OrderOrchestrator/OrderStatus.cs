namespace zeLaur.OrderService.OrderService
{
    public enum OrderStatus
    {
        Starting,
        GettingProducts,
        GettingAddress,
        ReservingProducts,
        CalculatingShipping,
        CalculatingFinalPrice,
        AwaitingPayment,
        SendShippingOrder,
        Completed
    }
}
