using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GDipSA51_Team5.Models
{
    public class Session
    {
        [Required]
        [MaxLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { set; get; }
        [Required]
        [MaxLength(36)]
        public int UserId { get; set; }

        public virtual User user { get; set; }

        [Required]
        [MaxLength(36)]
        public long Timestamp { get; set; }
    }
}
