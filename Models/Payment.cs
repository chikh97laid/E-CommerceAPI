using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models
{
    public enum enPaymentStatus { Paid, Pending, Failed}
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(20)]
        public string Method { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public enPaymentStatus PaymentStatus { get; set; } = enPaymentStatus.Pending;

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

    }
}



