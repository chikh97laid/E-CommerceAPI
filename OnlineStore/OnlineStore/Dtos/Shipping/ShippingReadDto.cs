using OnlineStore.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineStore.Dtos.Shipping
{
    public class ShippingReadDto
    {
        public int Id { get; set; }
        public string Carrier { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enShippingStatus ShippingStatus { get; set; }
    }
}
