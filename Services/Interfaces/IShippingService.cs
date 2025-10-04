using Microsoft.AspNetCore.Mvc;
using OnlineStore.Dtos.Shipping;
using OnlineStore.Models;
using OnlineStore.Services.Results;

namespace OnlineStore.Services.Interfaces
{
    public interface IShippingService : IService<ShippingReadDto, ShippingWriteDto>
    {
        Task<ServiceResult<ShippingReadDto?>> UpdateShipStatusAsync(int id, enShippingStatus status);
    }
}
