using AutoMapper;
using OnlineStore.Dtos;
using OnlineStore.Dtos.Category;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using OnlineStore.Services.Interfaces;
using OnlineStore.Services.Results;

namespace OnlineStore.Services.Implementations
{
    public class CategoryService : IService<CategoryReadDto, CategoryWriteDto>
    {
        private readonly IRepo<Category> _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryService(IRepo<Category> categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryReadDto>?> GetAllAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return categories == null ? null : _mapper.Map<IEnumerable<CategoryReadDto>>(categories);
        }

        public async Task<CategoryReadDto?> GetByIdAsync(int id)
        {
            var cat = await _categoryRepo.GetByIdAsync(id);
            return cat == null ? null : _mapper.Map<CategoryReadDto>(cat);
        }
        public async Task<ServiceResult<CategoryReadDto?>> AddAsync(CategoryWriteDto dto)
        {
            var category = _mapper.Map<Category>(dto);

            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveAsync();

            var categoryReadDto = _mapper.Map<CategoryReadDto>(category);

            return ServiceResult<CategoryReadDto?>.Ok(categoryReadDto);
        }

        public async Task<ServiceResult<CategoryReadDto?>> UpdateAsync(int id, CategoryWriteDto dto)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if(category == null) return ServiceResult<CategoryReadDto?>.Fail("Category not found");

            // يقوم AutoMapper بتحديث خصائص الكائن مباشرة
            _mapper.Map(dto, category);

            _categoryRepo.Update(category); 
            await _categoryRepo.SaveAsync();

            var categoryReadDto = _mapper.Map<CategoryReadDto>(category);

            return ServiceResult<CategoryReadDto?>.Ok(categoryReadDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cat = await _categoryRepo.GetByIdAsync(id);
            if (cat == null) return false;
            
            _categoryRepo.Delete(cat);
            await _categoryRepo.SaveAsync();

            return true;
        }

    }
}
