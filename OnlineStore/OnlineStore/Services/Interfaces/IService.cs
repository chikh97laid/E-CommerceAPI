using OnlineStore.Dtos.Category;
using OnlineStore.Services.Results;

namespace OnlineStore.Services.Interfaces
{
    public interface IService<T, TWriteDto> 
        where T : class 
        where TWriteDto : class
    {
        Task<IEnumerable<T>?> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<ServiceResult<T?>> AddAsync(TWriteDto dto);
        Task<ServiceResult<T?>> UpdateAsync(int id, TWriteDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
