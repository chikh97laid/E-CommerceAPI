using OnlineStore.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos.Payment
{
    public class PaymentReadDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public enPaymentStatus PaymentStatus { get; set; }
        public int OrderId { get; set; }
    }
}
