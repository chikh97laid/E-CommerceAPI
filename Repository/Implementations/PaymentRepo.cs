using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnlineStore.Data;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using System.Threading.Tasks;

namespace OnlineStore.Repository.Implementations
{
    public class PaymentRepo : IPaymentrepo
    {
        private readonly AppDbContext _context;

        public PaymentRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>?> GetAllAsync()
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.OrderItems)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.OrderItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AddAsync(Payment entity)
        {
            await _context.Payments.AddAsync(entity);
        }
        public void Update(Payment entity)
        {
            _context.Payments.Update(entity);
        }

        public void Delete(Payment entity)
        {
            _context.Payments.Remove(entity);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(int id)
        {
            return await _context.Payments.AnyAsync(x => x.Id == id);
        }

        public async Task<IDbContextTransaction> TransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
