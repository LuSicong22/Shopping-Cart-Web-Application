using System.Collections.Generic;
using System.Linq;
using GDipSA51_Team5.Models;
using Microsoft.AspNetCore.Mvc;
using GDipSA51_Team5.Data;

namespace GDipSA51_Team5.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly Team5_Db db;

        public PurchaseController(Team5_Db db)
        {
            this.db = db;
        }

        [HttpPost] //when the cart is submitted for purchase
        public IActionResult Transaction()
        {
            Session session = db.Sessions.FirstOrDefault(x => x.Id == HttpContext.Request.Cookies["sessionId"]);

            if (session == null)//if not login, redirect to login page
            {
                return RedirectToAction("Login", "Login");
            }

            int userid = session.UserId;

            List<Cart> carts = db.Carts.Where(x => x.UserId == userid).ToList();

            foreach (Cart item in carts) //for each item in the carts
            {
                ActivationC.AddActivationCode(item, db); //call a method to generate the activationcode and update to the orderdetail database (please refer to the ActivationC class)
                db.Carts.Remove(item);//remove the item from the Carts database
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Index()//When accessing purchase history
        {
            Session session = db.Sessions.FirstOrDefault(x => x.Id == HttpContext.Request.Cookies["sessionId"]);
            if (session == null)//if not login, redirect to login page
            {
                return RedirectToAction("Login", "Login");
            }
            int userid = session.UserId;

            List<Product> product = db.Products.ToList();

            List<OrderDetail> productDetails = db.OrderDetails.Where(x => x.UserId == userid).ToList();

            ViewData["purchaseinfo"] = productDetails;
            ViewData["sessionId"] = Request.Cookies["sessionId"];

            return View();
        }
    }
}
