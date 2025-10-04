using Microsoft.EntityFrameworkCore.Storage;
using OnlineStore.Models;

namespace OnlineStore.Repository.Interfaces
{
    public interface IPaymentrepo : IRepo<Payment>
    {
        Task<IDbContextTransaction> TransactionAsync();
    }
}
