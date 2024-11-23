using Microsoft.AspNetCore.Mvc;
using GameDrop.Data;

namespace GameDrop.Controllers
{
    public class OrderController : Controller
    {
        private readonly GameDropDBContext _db;

        public OrderController(GameDropDBContext db)
        {
            _db = db;
        }
    }
}