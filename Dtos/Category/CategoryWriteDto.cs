using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos.Category
{
    public class CategoryWriteDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
