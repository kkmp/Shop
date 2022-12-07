using System.ComponentModel.DataAnnotations;

namespace Shop.Data.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Category { get; set; }

        [Required]
        [MaxLength(1024)]
        public string? Description { get; set; }

        [Required]
        public string? Photo { get; set; }

        [Required]
        [Range(0.0, 100000.0)]
        public float Price { get; set; }
    }
}
