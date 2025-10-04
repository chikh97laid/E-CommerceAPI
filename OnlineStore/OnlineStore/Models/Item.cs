using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineStore.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        
        [Required,MaxLength(50)]
        public string Name { get; set; } = string.Empty;        
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public byte[]? Image { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;
        public ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Review>? Reviews { get; set; } = [];
    }
}
