
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using OnlineStore.Repository.Interfaces;

namespace OnlineStore.Repository.Implementations
{
    public class ItemRepo : IRepo<Item>
    {
        private readonly AppDbContext _context;

        public ItemRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>?> GetAllAsync()
        {
            var entities = await _context.Items  
                                         .AsNoTracking()
                                         .Include(i => i.Category)
                                         .ToListAsync();

            return entities;
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            var entity = await _context.Items
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(i => i.Id == id);

            return entity;
        }

        public async Task AddAsync(Item entity)
        {
            await _context.Items.AddAsync(entity); 
        }

        public void Update(Item entity)
        {
            _context.Items.Update(entity);
        }

        public void Patch(Item entity)
        {
            new JsonPatchDocument<Item>().ApplyTo(entity);
        }

        public void Delete(Item entity)
        {
            _context.Items.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(int id)
        {
            return await _context.Items.AnyAsync(c => c.Id == id);
        }

    }
}
