using System.ComponentModel.DataAnnotations;

namespace Shop.DTO.Product
{
    public class ProductUpdateDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MinLength(3)]
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
        [Range(0.01, 100000.0)]
        public float Price { get; set; }
    }
}
