using System.ComponentModel.DataAnnotations;

namespace GameDrop.Models
{
    public class GameDrop_ShoppingCart
    {
        [Key]
        public int ShoppingCartId { get; set; }
        public string? ShoppingCartName { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
