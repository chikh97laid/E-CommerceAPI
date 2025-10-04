using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }      
        public string? ReviewText { get; set; }
        
        [Range(1, 5, ErrorMessage = "Enter a number between 1 and 5")]
        public byte RatingScore { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }
        public Item? Item { get; set; }
    }
}
