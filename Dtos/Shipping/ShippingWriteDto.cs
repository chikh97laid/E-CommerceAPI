using OnlineStore.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineStore.Dtos.Shipping
{
    public class ShippingWriteDto
    {
        [Required]
        [MaxLength(20)]
        public string Carrier { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string TrackingNumber { get; set; } = string.Empty;
       
    }
}
