using OnlineStore.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos.Item
{
    public class ItemWriteDto
    {
        [Required, MaxLength(50)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public int CategoryId { get; set; }
        
    }
}
