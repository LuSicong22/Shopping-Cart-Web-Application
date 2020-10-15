using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GDipSA51_Team5.Data;
using GDipSA51_Team5.Models;

namespace GDipSA51_Team5.Controllers
{
    public class GalleryController : Controller
    {
        private readonly Team5_Db db;

        public GalleryController(Team5_Db db)
        {
            this.db = db;
        }

        public IActionResult Index(string searchString)
        {
            string sessionId = Request.Cookies["sessionId"];
            ViewData["sessionId"] = sessionId;

            Session session = db.Sessions.FirstOrDefault(x => x.Id == sessionId);
            if (session != null)
            {
                ViewData["Username"] = db.Users.FirstOrDefault(x => x.UserId == session.UserId).Username;
            }
            else
            {
                ViewData["Username"] = "Guest";
            }

            ViewData["Products"] = db.Products.Where(s => (s.Name.Contains(searchString) || s.Description.Contains(searchString)) || searchString == null).ToList();

            return View();
        }
    }
}
