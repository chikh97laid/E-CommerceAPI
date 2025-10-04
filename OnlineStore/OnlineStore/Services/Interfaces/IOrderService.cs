using OnlineStore.Dtos.Order;
using OnlineStore.Services.Results;

namespace OnlineStore.Services.Interfaces
{
    public interface IOrderService : IService<OrderReadDto, OrderWriteDto>
    {
        Task<ServiceResult<OrderReadDto?>> CancelOrder(int id);
    }
}
