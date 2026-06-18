using OnlineStore.Models;
using System.Linq.Expressions;

namespace OnlineStore.Repository.Interfaces
{
    public interface IItemRepo : IRepo<Item>
    {
        Task<IEnumerable<Item>?> GetAllAsync(int? categoryId = null);    
        
    }
}
