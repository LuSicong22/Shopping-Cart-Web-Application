using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using GDipSA51_Team5.Models;
using GDipSA51_Team5.Data;
using System.Text.Json;

namespace GDipSA51_Team5.Controllers
{
    public class CartController : Controller
    {
        private readonly Team5_Db db;
        private readonly CartItems cartitems;
        //get the database and cartitems of guests
        public CartController(Team5_Db db, CartItems cartitems)
        {
            this.db = db;
            this.cartitems = cartitems;
        }

        [HttpPost]
        public string Cart([FromBody] ChangeInput change)//receive JSON object from Cart.js when the number in the cart is changed
        {
            Session session = db.Sessions.FirstOrDefault(x => x.Id == HttpContext.Request.Cookies["sessionId"]);
            int tempProd = int.Parse(change.ProductId);
            int tempValue = int.Parse(change.Value);

            if (session == null)//if the user is not login
            {
                string fakeUserId = HttpContext.Request.Cookies["cartItems"];//get the guest fakeuserid
                CartItem cartitem = cartitems.map[fakeUserId];//get the guest cartitem information based on the fakeuserid (same idea as the sessions in workshop)

                //inside cartitem class is a list of KeyValuePair of <int,int> --> can refer to the cartitem class file for more information
                //KeyValuePair is used to store the productid and quantity of each of the products.
                for (int i = 0; i < cartitem.item.Count(); i++)
                {
                    if (cartitem.item[i].Key == tempProd) //update the quantity of the KeyValuePair with the productId passed in
                    {
                        cartitem.item.Remove(cartitem.item[i]);
                        cartitem.item.Add(new KeyValuePair<int, int>(tempProd, tempValue));
                    }
                }
            }
            else //else if the user is login
            {
                int userid = session.UserId;
                List<Cart> carts = db.Carts.Where(x => x.UserId == userid).ToList();

                foreach (Cart item in carts)//Update the cart of the user with the new value into the databse
                {
                    if (item.ProductId == tempProd)
                    {
                        item.Quantity = tempValue;
                        db.SaveChanges();
                    }
                };
            }

            object data = new
            {
                status = "success"
            };

            return JsonSerializer.Serialize(data);
        }

        public IActionResult Cart()//HttpGET on the cart() action
        {
            Session session = db.Sessions.FirstOrDefault(x => x.Id == HttpContext.Request.Cookies["sessionId"]);

            if (session == null) //if the user is not login
            {
                string fakeUserId = HttpContext.Request.Cookies["cartItems"];
                CartItem cartitem = null;
                if (fakeUserId != null)//try to get the cartitem by using the fakeUserId, will return null to cartitem if no record is found for the fakeUserId
                {
                    cartitems.map.TryGetValue(fakeUserId, out cartitem);
                }
                if (fakeUserId == null || cartitem == null)//if the guest has no fakeUserId or no matching of fakeUserId
                {
                    HttpContext.Response.Cookies.Delete("cartItems");
                    return View();//directly returning the View() ---> will be an empty page
                }

                List<Product> details = new List<Product>();
                foreach (KeyValuePair<int, int> item in cartitem.item)//retrieve the product details of each KeyValuePair under the guest.(information needed in the html page)
                {
                    Product detail = db.Products.FirstOrDefault(x => x.ProductId == item.Key);
                    details.Add(detail);
                }
                ViewData["details"] = details;
                ViewData["KeyValue"] = cartitem.item;
                ViewData["sessionId"] = Request.Cookies["sessionId"];

                return View();
            }
            else//else if the user is login
            {
                int userid = session.UserId;
                List<Cart> carts = db.Carts.Where(x => x.UserId == userid).ToList();//retrieve the carts of the user

                ViewData["productList"] = carts;
                ViewData["sessionId"] = Request.Cookies["sessionId"];

                return View();
            }
        }

        [HttpPost]
        public JsonResult RemoveItem([FromBody] Addinput productId)
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            int id = int.Parse(productId.ProductId);
            Session session = db.Sessions.FirstOrDefault(x => x.Id == sessionId);

            //if the user is not login, their session will be null
            if (session == null)
            {
                //fakeUserId is a cookies that will be sent to the guest(not login) to track their activity
                string fakeUserId = HttpContext.Request.Cookies["cartItems"];
                CartItem cartitem = cartitems.map[fakeUserId];

                //inside cartitem class is a list of KeyValuePair of <int,int> --> can refer to the cartitem class file for more information
                //KeyValuePair is used to store the productid and quantity of each of the products.
                for (int i = 0; i < cartitem.item.Count(); i++)
                {
                    if (cartitem.item[i].Key == id) //get the KeyValuePair of that particular product and increment the quantity by 1
                    {
                        cartitem.item.Remove(cartitem.item[i]);
                    }
                }
                cartitems.map[fakeUserId] = cartitem; //update the cartitems (cartitems is a singleton object that store cartitem of guest)(Same idea as sessions in the workshop)
            }
            else //else if the user has login
            {
                List<Cart> carts = db.Carts.Where(x => x.UserId == session.UserId).ToList(); //get the carts information of the user

                foreach (Cart item in carts)//get the cart row and increment the quantity by 1
                {
                    if (item.ProductId == id)
                    {
                        db.Carts.Remove(item);
                        db.SaveChanges();
                    }
                }
            }
            return Json(new
            {
                status = "success"
            });
        }
    }
}