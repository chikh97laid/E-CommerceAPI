using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Dtos.Category;
using OnlineStore.Dtos.Item;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using OnlineStore.Services.Interfaces;
using OnlineStore.Services.Results;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineStore.Services.Implementations
{
    public class ItemService : IitemService
    {
        private readonly IRepo<Item> _itemRepo;
        private readonly IMapper _mapper;
        private readonly IRepo<Category> _categoryRepo;

        public ItemService(IRepo<Item> itemRepo, IMapper mapper, IRepo<Category> categoryRepo)
        {
            _itemRepo = itemRepo;
            _mapper = mapper;
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<ItemReadDto>?> GetAllAsync()
        {
            var items = await _itemRepo.GetAllAsync();
            return items == null ? null : _mapper.Map<IEnumerable<ItemReadDto>>(items);
        }


        public async Task<ItemReadDto?> GetByIdAsync(int id)
        {
            var item = await _itemRepo.GetByIdAsync(id);
            return item == null ? null : _mapper.Map<ItemReadDto>(item);
        }

        public async Task<byte[]?> GetItemImageAsync(int id)
        {
            var item = await _itemRepo.GetByIdAsync(id);
            if (item == null || item.Image == null)
            {
                return null;
            }

            return item.Image;
        }

        public async Task<ServiceResult<ItemReadDto?>> AddAsync(ItemWriteDto dtItem)
        {
            if (!await _categoryRepo.IsExistAsync(dtItem.CategoryId))
            {
                return ServiceResult<ItemReadDto?>.Fail("Category not found");
            }

            var item = _mapper.Map<Item>(dtItem);

            if (dtItem.Image != null)
            {
                using var stream = new MemoryStream();
                await dtItem.Image.CopyToAsync(stream);
                item.Image = stream.ToArray();
            }

            await _itemRepo.AddAsync(item);
            await _itemRepo.SaveAsync();

            var itemReadDto = _mapper.Map<ItemReadDto>(item);
            //itemReadDto.ImageUrl = $"/api/items/{item.Id}/image"; // this to call the end point GetItemImage

            return ServiceResult<ItemReadDto?>.Ok(itemReadDto);
        }

        public async Task<ServiceResult<ItemReadDto?>> UpdateAsync(int id, ItemWriteDto dto)
        {
            if (!await _categoryRepo.IsExistAsync(dto.CategoryId))
            {
                return ServiceResult<ItemReadDto?>.Fail("Category not found");
            }

            var item = await _itemRepo.GetByIdAsync(id);

            if (item == null) return ServiceResult<ItemReadDto?>.Fail("Item not found");

            _mapper.Map(dto, item);
            
            if(dto.Image != null)
            {
                using var stream = new MemoryStream();
                await dto.Image.CopyToAsync(stream);
                item.Image = stream.ToArray();
            }

            _itemRepo.Update(item);
            await _itemRepo.SaveAsync();

            var itemReadDto = _mapper.Map<ItemReadDto>(item);
            //itemReadDto.ImageUrl = $"/api/items/{item.Id}/image"; // this to call the end point GetItemImage

            return ServiceResult<ItemReadDto?>.Ok(itemReadDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _itemRepo.GetByIdAsync(id);
            if (item == null) return false;

            _itemRepo.Delete(item);
            await _itemRepo.SaveAsync();

            return true;
        }

    }
}
