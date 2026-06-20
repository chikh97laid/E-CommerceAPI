using OnlineStore.Models;

namespace OnlineStore.Repository.Interfaces
{
    public interface ICustomerRepo : IRepo<Customer>
    {
        Task<bool> IsEmailExistAsync(string email, int? excludeId = null);
    }
}
