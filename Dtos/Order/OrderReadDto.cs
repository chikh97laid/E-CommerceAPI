using OnlineStore.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineStore.Dtos.Order
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enOrderStatus OrderStatus { get; set; }
        public int CustomerId { get; set; }
        public int? ShippingId { get; set; }
        public decimal? Total { get; set; }
        public ICollection<OrderItemReadDto> Items { get; set; } = [];
    }

    public class OrderItemReadDto
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
