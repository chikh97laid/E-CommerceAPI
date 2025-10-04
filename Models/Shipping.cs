using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public enum enShippingStatus { Processing , OutForDelivery, Delivered, ReturnToSender, OnHold, Delayed, Lost }
    public class Shipping
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Carrier { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string TrackingNumber { get; set; } = string.Empty;
        public enShippingStatus ShippingStatus { get; set; }
    }
}
