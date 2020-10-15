using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GDipSA51_Team5.Models
{
    public class Product
    {

        public Product()
        {
            ProductDetails = new HashSet<OrderDetail>();
        }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(64)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Url { get; set; }

        public string GalleryTypeId { get; set; }

        public virtual ICollection<OrderDetail> ProductDetails { get; set; }

    }
}
