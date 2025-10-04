using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models
{
    public enum enOrderStatus{ Pending, Processing, Shipped, Delivered, Cancelled, Refunded }

    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }       
        public enOrderStatus OrderStatus { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Payment? Payment { get; set; }

        [ForeignKey(nameof(Shipping))]
        public int? ShippingId { get; set; }
        public Shipping? Shipping { get; set; }
    }
}
