using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GDipSA51_Team5.Models
{
    public class User
    {

        public User()
        {
            ProductDetails = new HashSet<OrderDetail>();
        }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(15)]
        public string Password { get; set; }

        public virtual ICollection<OrderDetail> ProductDetails { get; set; }
    }
}
