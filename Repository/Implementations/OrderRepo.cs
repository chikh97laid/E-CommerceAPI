using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using System.Threading.Tasks;

namespace OnlineStore.Repository.Implementations
{
    public class OrderRepo : IRepo<Order>
    {
        private readonly AppDbContext _context;

        public OrderRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>?> GetAllAsync()
        {
            var entities = await _context.Orders.Include(o => o.OrderItems)
                 .ThenInclude(oi => oi.Item)
                 .AsNoTracking()
                 .ToListAsync();

            return entities;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var entity = await _context.Orders.Include(o => o.OrderItems)
               .ThenInclude(oi => oi.Item)
               .AsNoTracking()
               .FirstOrDefaultAsync(o => o.Id == id);

            return entity;
        }

        public async Task AddAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);
            
        }

        public void Update(Order entity)
        {
            _context.Orders.Update(entity);
        }

        public void Delete(Order entity)
        {
            _context.Orders.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsExistAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }

    }
}
