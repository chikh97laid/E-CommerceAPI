using OnlineStore.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos.Payment
{
    public class PaymentWriteDto
    {
        [Required, MaxLength(20)]
        public string Method { get; set; } = string.Empty;
        
        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public int OrderId { get; set; }
    }
}
