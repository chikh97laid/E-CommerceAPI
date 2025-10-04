using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using System.Threading.Tasks;

namespace OnlineStore.Repository.Implementations
{
    public class ShippingRepo : IShippingRepo
    {
        private readonly AppDbContext _context;

        public ShippingRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shipping>?> GetAllAsync()
        {
            var entities = await _context.Shippings
                                          .AsNoTracking()
                                          .ToListAsync();
            return entities;
        }

        public async Task<Shipping?> GetByIdAsync(int id)
        {
            var entity = await _context.Shippings
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            
            return entity;
        }

        public async Task AddAsync(Shipping entity)
        {
            await _context.Shippings.AddAsync(entity);
        }

        public void Update(Shipping entity)
        {
            _context.Shippings.Update(entity);
        }

        public void Delete(Shipping entity)
        {
            _context.Shippings.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(int id)
        {
            return await _context.Shippings.AnyAsync(o => o.Id == id); 
        }

        public void UpdateShippingStatus(Shipping shipping)
        {
            _context.Attach(shipping);
            _context.Entry(shipping).Property(o => o.ShippingStatus).IsModified = true;
        }
    }
}
