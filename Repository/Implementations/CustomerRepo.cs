using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using System.Threading.Tasks;

namespace OnlineStore.Repository.Implementations
{
    public class CustomerRepo : IRepo<Customer>
    {
        private readonly AppDbContext _context;
        public CustomerRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>?> GetAllAsync()
        {
            var entities = await _context.Customers
                             .AsNoTracking()
                             .ToListAsync();

            return entities;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var entity = await _context.Customers
                           .AsNoTracking()
                           .FirstOrDefaultAsync(i => i.Id == id);

            return entity;
        }

        public async Task AddAsync(Customer entity)
        {
            await _context.Customers.AddAsync(entity);
        }

        public void Update(Customer entity)
        {
            _context.Customers.Update(entity);
        }

        public void Delete(Customer entity)
        {
            _context.Customers.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsExistAsync(int id)
        {
            return await _context.Customers.AnyAsync(c => c.Id == id);
        }
    }
}
