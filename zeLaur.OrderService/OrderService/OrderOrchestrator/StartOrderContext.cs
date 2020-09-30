namespace zeLaur.OrderService.OrderService.OrderOrchestrator
{
    public class StartOrderContext
    {
        public string CorrelationId { get; set; }
        public string ShoppingCartId { get; set; }
        public string UserId { get; set; }
    }
}
