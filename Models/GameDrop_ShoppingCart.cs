namespace GameDrop.Models
{
    public class GameDrop_ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public string? ShoppingCartName { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
