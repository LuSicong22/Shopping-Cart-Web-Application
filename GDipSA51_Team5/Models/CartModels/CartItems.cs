using System.Collections.Generic;

namespace GDipSA51_Team5.Models
{
    public class CartItems
    {
        public Dictionary<string, CartItem> map { get; set; }

        public CartItems()
        {
            map = new Dictionary<string, CartItem>();
        }
    }
}