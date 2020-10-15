using GDipSA51_Team5.Data;
using System;
using System.ComponentModel.DataAnnotations;
using GDipSA51_Team5.Models;

namespace GDipSA51_Team5.Models
{
    public class OrderDetail
    {
        [Required]
        public int SerialNo { get; set; }

        [MaxLength(20)]
        [Required]
        public string ActivationCode { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        public virtual Product Products { get; set; }
        public virtual User Users { get; set; }
    }

    public class ActivationC
    {
        public static void AddActivationCode(Cart cart, Team5_Db db)
        {
            for (int i = 0; i < cart.Quantity; i++)
            {
                OrderDetail order = new OrderDetail
                {
                    ActivationCode = Guid.NewGuid().ToString().Substring(3, 15),
                    UserId = cart.UserId,
                    ProductId = cart.ProductId,
                    PurchaseDate = DateTime.Today.Date
                };

                db.Add(order);
                db.SaveChanges();
            }

            return;
        }
    }

}
