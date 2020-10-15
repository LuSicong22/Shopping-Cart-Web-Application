using System;
using System.Collections.Generic;
using System.Linq;
using GDipSA51_Team5.Models;
using Microsoft.AspNetCore.Mvc;
using GDipSA51_Team5.Data;

namespace GDipSA51_Team5.Controllers
{
    public class AddController : Controller
    {
        private readonly Team5_Db db;
        private readonly CartItems cartitems;
        //get the database and cartitems of guests
        public AddController(Team5_Db db, CartItems cartitems)
        {
            this.db = db;
            this.cartitems = cartitems;
        }

        //receive JSON data from Add.js. (When an item is added to the cart from gallery)
        public JsonResult Addin([FromBody] Addinput addinput)
        {
            Session session = db.Sessions.FirstOrDefault(x => x.Id == HttpContext.Request.Cookies["sessionId"]);

            bool newItem = true;
            int productid = int.Parse(addinput.ProductId);
            int total = 0;

            //if the user is not login, their session will be null
            if (session == null)
            {
                //fakeUserId is a cookies that will be sent to the guest(not login) to track their activity
                string fakeUserId = HttpContext.Request.Cookies["cartItems"];
                CartItem cartitem;

                //if they dont have this cookies(null), create a cookie of fakeUserId and send to them
                if (fakeUserId == null)
                {
                    fakeUserId = Guid.NewGuid().ToString();
                    Response.Cookies.Append("cartItems", fakeUserId);
                    cartitem = new CartItem();
                }
                else //else if they have the fakeUserId cookie, find their record from the cartitems
                {
                    cartitem = null;
                    cartitems.map.TryGetValue(fakeUserId, out cartitem);//check if the fakeUserId is in our record or not
                    if (cartitem == null)//if its not in our record
                    {
                        cartitem = new CartItem();
                    }
                }
                //inside cartitem class is a list of KeyValuePair of <int,int> --> can refer to the cartitem class file for more information
                //KeyValuePair is used to store the productid and quantity of each of the products.
                foreach (KeyValuePair<int, int> item in cartitem.item)
                {
                    if (item.Key == productid) //to check if the product has been added before
                    {
                        newItem = false;
                    }
                }
                if (cartitem.item.Count() == 0 || newItem == true) //if its a new product or the list is empty
                {
                    cartitem.item.Add(new KeyValuePair<int, int>(productid, 1)); //Add a new KeyValuePair of this productid with quantity of 1
                }
                else
                {
                    for (int i = 0; i < cartitem.item.Count(); i++)
                    {
                        if (cartitem.item[i].Key == productid) //get the KeyValuePair of that particular product and increment the quantity by 1
                        {
                            int quantity = cartitem.item[i].Value;
                            cartitem.item.Remove(cartitem.item[i]);
                            cartitem.item.Add(new KeyValuePair<int, int>(productid, quantity + 1));
                            break;
                        }
                    }
                }
                cartitems.map[fakeUserId] = cartitem; //update the cartitems (cartitems is a singleton object that store cartitem of guest)(Same idea as sessions in the workshop)

                //this part is to get the total quantity of products that the guest has. So that can be reflected on the cart image.
                for (int i = 0; i < cartitem.item.Count(); i++)
                {
                    total += cartitem.item[i].Value;
                }
            }
            else //else if the user has login
            {
                List<Cart> carts = db.Carts.Where(x => x.UserId == session.UserId).ToList(); //get the carts information of the user
                foreach (Cart item in carts) //check if its a item or not
                {
                    if (item.ProductId == productid)
                    {
                        newItem = false;
                    }
                }
                if (carts.Count() == 0 || newItem == true) //if its a new item or the user has no items in his cart
                {
                    Cart item = new Cart
                    {
                        UserId = session.UserId,
                        ProductId = productid,
                        Quantity = 1
                    };
                    db.Add(item);
                    db.SaveChanges();//save the item to the user cart database
                }
                else //else if its not a new item
                {
                    foreach (Cart item in carts)//get the cart row and increment the quantity by 1
                    {
                        if (item.ProductId == productid)
                        {
                            item.Quantity += 1;
                            db.SaveChanges();
                        }
                    }
                }

                //this part is to get the total quantity of products that the user has. So that can be reflected on the cart image.
                carts = db.Carts.Where(x => x.UserId == session.UserId).ToList();
                foreach (Cart item in carts)
                {
                    total += item.Quantity;
                }
            }
            //return the total as JSON to the Add.js
            return Json(new
            {
                status = "success",
                total = total
            });
        }
    }
}
