using OnlineStore.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos.Review
{
    public class ReviewReadDto
    {
        public int Id { get; set; }
        public string? ReviewText { get; set; }
        public byte RatingScore { get; set; }
        public DateTime TimeStamp { get; set; }
        public int CustomerId { get; set; }
        public int ItemId { get; set; }
    }
}
