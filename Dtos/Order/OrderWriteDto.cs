using OnlineStore.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineStore.Dtos.Order
{
    public class OrderWriteDto
    {
        [Required]
        public int OrderNumber { get; set; }
    
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int CustomerId { get; set; }
        
        public int? ShippingId { get; set; }
        public ICollection<OrderItemWriteDto> Items { get; set; } = [];
    }

    public class OrderItemWriteDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
