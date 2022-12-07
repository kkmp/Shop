using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Data.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Product? Product { get; set; }

        [ForeignKey("Product")]
        public Guid? ProductId { get; set; }

        [Required]
        public IdentityUser? User { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Status { get; set; }

        [Required]
        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [Required]
        [MaxLength(50)]
        public string? PaymentStatus { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Address { get; set; }
    }
}
