using System.ComponentModel.DataAnnotations;

namespace GDipSA51_Team5.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public virtual Product product { get; set; }

    }
}


