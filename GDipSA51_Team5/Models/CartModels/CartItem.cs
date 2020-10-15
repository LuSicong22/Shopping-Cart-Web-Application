using System.Collections.Generic;

namespace GDipSA51_Team5.Models
{
    public class CartItem
    {
        public List<KeyValuePair<int, int>> item { get; set; }
        public CartItem()
        {
            item = new List<KeyValuePair<int, int>>();
        }
    }
}
