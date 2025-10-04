using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos.Category
{
    public class CategoryReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
