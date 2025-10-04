using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos.Item
{
    public class ItemReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }

    }
}
