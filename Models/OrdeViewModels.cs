using System.ComponentModel.DataAnnotations;
using GameDrop.Models;
//make order and order details to ordercontroller
namespace GameDrop.ViewModels
{
    public class OrderViewModel
    {
        public GameDrop_Order? Order { get; set; }
        public GameDrop_OrderDetails? OrderDetails { get; set; }
    }
}

