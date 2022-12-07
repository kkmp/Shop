using System.ComponentModel.DataAnnotations;

namespace Shop.DTO
{
    public class IdDTO
    {
        [Required]
        public Guid? Id { get; set; }
    }
}
