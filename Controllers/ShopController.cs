using Microsoft.AspNetCore.Mvc;

namespace GameDrop.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
