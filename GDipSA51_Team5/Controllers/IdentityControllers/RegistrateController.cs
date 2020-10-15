using System.Linq;
using GDipSA51_Team5.Models;
using Microsoft.AspNetCore.Mvc;
using GDipSA51_Team5.Data;

namespace GDipSA51_Team5.Controllers
{
    public class RegistrateController : Controller
    {
        private readonly Team5_Db db;

        public RegistrateController(Team5_Db db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(string username, string NewPWD, string ConfirmedPWD)
        {
            User user = db.Users.FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                if (NewPWD == ConfirmedPWD)
                {
                    User users = new User()
                    {
                        Username = username,
                        Password = NewPWD,
                    };
                    db.Add(users);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    ViewData["errMsg"] = "Please enter a consistent password";
                    return View("Index");
                }
            }
            else
            {
                ViewData["errMsg"] = "user has existed";
                return View("Index");
            }

           
        }
    }
    
}