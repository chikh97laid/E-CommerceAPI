using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using System.Threading.Tasks;

namespace OnlineStore.Repository.Implementations
{
    public class ReviewRepo : IReviewRepo
    {

        private readonly AppDbContext _context;

        public ReviewRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>?> GetAllAsync()
        {
            return await _context.Reviews
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Review entity)
        {
            await _context.Reviews.AddAsync(entity);
        }

        public void Update(Review entity)
        {
            _context.Reviews.Update(entity);
        }

        public void Delete(Review entity)
        {
            _context.Reviews.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(int id)
        {
            return await _context.Reviews.AnyAsync(x => x.Id == id);
        }
        public async Task<bool> IsForeignKeysRepeated(int customerId, int itemId)
        {
            return await _context.Reviews.AnyAsync(x => x.CustomerId == customerId && x.ItemId == itemId);
        }

    }
}
