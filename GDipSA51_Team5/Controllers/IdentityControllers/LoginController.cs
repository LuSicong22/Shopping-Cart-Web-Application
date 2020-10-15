using System;
using System.Collections.Generic;
using System.Linq;
using GDipSA51_Team5.Models;
using Microsoft.AspNetCore.Mvc;
using GDipSA51_Team5.Data;

namespace GDipSA51_Team5.Controllers
{
    public class LoginController : Controller
    {
        //get the database and cartitems of guests
        private readonly Team5_Db db;
        private readonly CartItems cartitems;

        public LoginController(Team5_Db db, CartItems cartitems)
        {
            this.db = db;
            this.cartitems = cartitems;
        }

        public IActionResult Login()
        {
            return View();
        }

        /* check if username and password are both in Users db*/
        public IActionResult Authenticate(string username, string password)
        {
            /* get record in Users db */
            User user = db.Users.FirstOrDefault(x => x.Username == username
                                                  && x.Password == password);

            /* if no record from Users db was returned, show error msg
               else (record from Users db was returned), user is authenticated*/

            /* authenication failed*/ 
            //--- show error msg in Login page
            if (user == null)
            {
                ViewData["errMsg"] = "no such user or incorrect password";
                return View("Login");
            }

            /* authentication passed*/

            //-- create new session record in Sessions db
            Session session = new Session()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.UserId,
                Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
            };
            db.Sessions.Add(session);
            db.SaveChanges();

            //-- create a new cookie in the user's browser
            Response.Cookies.Append("sessionId", session.Id);

            // check if a cookie was sent to identify guest who added to cart
            string fakeUserId = HttpContext.Request.Cookies["cartItems"];

            // if user added to cart before logging in,
            // add any new items to their existing cart 
            // and return them to the View Cart page 
            if (fakeUserId != null)
            {
                AddNewItemsToCart(session, fakeUserId);

                HttpContext.Response.Cookies.Delete("cartItems"); //delete the cookie that was used to track the guest(fakeUserId)
                return RedirectToAction("Cart", "Cart");
            }

            // else (user did not to cart before logging in,)
            // return user to Gallery page
            Response.Cookies.Append("username", username);
            return RedirectToAction("Index", "Gallery");
        }

        private void AddNewItemsToCart(Session session, string fakeUserId)
        {
            //-- get existing cart of user from Carts db 
            List<Cart> carts = db.Carts.Where(x => x.UserId == session.UserId).ToList();

            //-- get the list of items (productid and quantity) added to cart by this guest before login
            CartItem cartitem = cartitems.map[fakeUserId];

            // for each new item in the cart
            // add them to the existing cart of the user
            foreach (KeyValuePair<int, int> keyValue in cartitem.item)
            {
                // check if item is in existing cart 
                bool newItem = true;

                foreach (Cart cart in carts)
                    if (cart.ProductId == keyValue.Key)
                        newItem = false;

                // if item a new item
                // create a new record in the cart and save 
                if (newItem == true)
                {
                    Cart item = new Cart 
                    {
                        UserId = session.UserId,
                        ProductId = keyValue.Key,
                        Quantity = keyValue.Value
                    };
                    db.Add(item);
                    db.SaveChanges();
                }

                // if item is already in the cart
                // look through the existing card for the record for the product 
                // update the quantity of the product and save to Carts db
                foreach (Cart item in carts)
                {
                    if (item.ProductId == keyValue.Key)
                    {
                        item.Quantity += keyValue.Value;
                        db.SaveChanges();
                    }
                }
            }
        } // end of AddNewItemsToCart()


    } // end of LoginController
} // end of GDipSA51_Team5.Controllers
