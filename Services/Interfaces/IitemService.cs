using Microsoft.AspNetCore.Mvc;
using OnlineStore.Dtos.Item;
using OnlineStore.Models;

namespace OnlineStore.Services.Interfaces
{
    public interface IitemService : IService<ItemReadDto, ItemWriteDto>
    {
        Task<byte[]?> GetItemImageAsync(int id);   
    }
}
