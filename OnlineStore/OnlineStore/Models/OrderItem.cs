using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int Quentity {  get; set; }
        public decimal Price { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
    }
}
