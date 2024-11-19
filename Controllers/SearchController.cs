using Microsoft.AspNetCore.Mvc;
using GameDrop.Data;

namespace GameDrop.Controllers
{
    public class SearchController : Controller
    {
        private readonly GameDropDBContext _db;

        public SearchController(GameDropDBContext db)
        {
            _db = db;
        }

        
    }
}
