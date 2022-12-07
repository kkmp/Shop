using System.ComponentModel.DataAnnotations;

namespace Shop.DTO.Order
{
    public class OrderCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Address { get; set; }
    }
}
