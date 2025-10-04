using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
       
        public ICollection<Item>? Items { get; set; }
    }
}
