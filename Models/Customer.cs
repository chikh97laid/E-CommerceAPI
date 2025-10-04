using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
                
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required, MaxLength(100)]
        public string Address { get; set; } = string.Empty;
        
        [Required, MaxLength(20)]
        public string City { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Region { get; set; }
        
        [Required, MaxLength (20)]
        public string PostalCode { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Country { get; set; } = string.Empty;
        public ICollection<Order>? Orders { get; set; }
    }
}
