using Microsoft.AspNetCore.Mvc;
using GDipSA51_Team5.Data;
using GDipSA51_Team5.Models;

namespace GDipSA51_Team5.Controllers
{
    public class LogoutController : Controller
    {
        private readonly Team5_Db db;

        public LogoutController(Team5_Db db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            db.Sessions.Remove(new Session()
            {
                Id = sessionId
            });

            // ask browser to remove the user's sessionId
            // the next time the browser will not send us any
            // sessionId for this user
            HttpContext.Response.Cookies.Delete("sessionId");

            // direct user back to our default gallery
            return RedirectToAction("Index", "Gallery");
        }
    }
}
