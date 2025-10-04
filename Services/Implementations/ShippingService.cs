using AutoMapper;
using OnlineStore.Dtos.Review;
using OnlineStore.Dtos.Shipping;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using OnlineStore.Services.Interfaces;
using OnlineStore.Services.Results;

namespace OnlineStore.Services.Implementations
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingRepo _shippingRepo;
        private readonly IMapper _mapper;

        public ShippingService(IShippingRepo shippingRepo, IMapper mapper)
        {
            _shippingRepo = shippingRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShippingReadDto>?> GetAllAsync()
        {
            var shipping = await _shippingRepo.GetAllAsync();
            return shipping == null ? null : _mapper.Map<IEnumerable<ShippingReadDto>>(shipping);
        }

        public async Task<ShippingReadDto?> GetByIdAsync(int id)
        {
            var shipping = await _shippingRepo.GetByIdAsync(id);
            return shipping == null ? null : _mapper.Map<ShippingReadDto>(shipping);
        }

        public async Task<ServiceResult<ShippingReadDto?>> AddAsync(ShippingWriteDto dto)
        {
            var shipping = _mapper.Map<Shipping>(dto);
            shipping.ShippingStatus = enShippingStatus.Processing;

            await _shippingRepo.AddAsync(shipping);
            await _shippingRepo.SaveAsync();

            return ServiceResult<ShippingReadDto?>.Ok(_mapper.Map<ShippingReadDto>(shipping));
        }
        public async Task<ServiceResult<ShippingReadDto?>> UpdateAsync(int id, ShippingWriteDto dto)
        {
            var shipping = await _shippingRepo.GetByIdAsync(id);
            if(shipping == null)
            {
                return ServiceResult<ShippingReadDto?>.Fail("Shipping not found");
            }

             _mapper.Map(dto, shipping);

            _shippingRepo.Update(shipping);
            await _shippingRepo.SaveAsync();

            return ServiceResult<ShippingReadDto?>.Ok(_mapper.Map<ShippingReadDto>(shipping));
        }
        public async Task<ServiceResult<ShippingReadDto?>> UpdateShipStatusAsync(int id, enShippingStatus status)
        {
            if (!await _shippingRepo.IsExistAsync(id)) return ServiceResult<ShippingReadDto?>.Fail("Shipping not found");
            var shipping = new Shipping()
            {
                Id = id,
                ShippingStatus = status
            };

            _shippingRepo.UpdateShippingStatus(shipping);
            await _shippingRepo.SaveAsync();

            return ServiceResult<ShippingReadDto?>.Ok(new ShippingReadDto { ShippingStatus = status});
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var shipping = await _shippingRepo.GetByIdAsync(id);
            if(shipping == null) return false;

            _shippingRepo.Delete(shipping);
            await _shippingRepo.SaveAsync();
            
            return true;
        }

    }
}
