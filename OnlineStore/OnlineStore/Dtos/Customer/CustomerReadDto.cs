using OnlineStore.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos.Customer
{
    public class CustomerReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? Region { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
