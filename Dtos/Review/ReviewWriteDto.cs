using OnlineStore.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos.Review
{
    public class ReviewWriteDto
    {
        public string? ReviewText { get; set; }

        [Required, Range(1, 5, ErrorMessage = "Enter a number between 1 and 5")]
        public byte RatingScore { get; set; }
        
        [Required]
        public DateTime TimeStamp { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        
        [Required]
        public int ItemId { get; set; }
    }
}
