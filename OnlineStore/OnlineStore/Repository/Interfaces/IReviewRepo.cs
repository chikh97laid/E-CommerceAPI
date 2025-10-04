using OnlineStore.Models;

namespace OnlineStore.Repository.Interfaces
{
    public interface IReviewRepo : IRepo<Review>
    {
        Task<bool> IsForeignKeysRepeated(int customerId, int itemId);
    }
}
