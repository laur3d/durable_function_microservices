namespace zeLaur.OrderService.OrderService
{
    public enum OrderStatus
    {
        Starting,
        GettingProducts,
        GettingAddress,
        ReservingProducts,
        CalculatingShipping,
        AwaitingPayment,
        SendShippingOrder,
        Completed
    }
}
