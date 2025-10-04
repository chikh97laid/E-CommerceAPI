using AutoMapper;
using OnlineStore.Dtos.Payment;
using OnlineStore.Dtos.Review;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using OnlineStore.Services.Interfaces;
using OnlineStore.Services.Results;
using System.Threading.Tasks;

namespace OnlineStore.Services.Implementations
{
    public class ReviewService : IService<ReviewReadDto, ReviewWriteDto>
    {
        private readonly IReviewRepo _reviewRepo;
        private readonly IRepo<Customer> _customerRepo;
        private readonly IRepo<Item> _itemRepo;
        private readonly IMapper _mapper;

         public ReviewService(IReviewRepo reviewRepo, IMapper mapper, IRepo<Customer> customerRepo, IRepo<Item> itemRepo)
        {
            _reviewRepo = reviewRepo;
            _customerRepo = customerRepo;
            _itemRepo = itemRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewReadDto>?> GetAllAsync()
        {
            var reviews = await _reviewRepo.GetAllAsync();
            return reviews == null ? null : _mapper.Map<IEnumerable<ReviewReadDto>>(reviews);
        }
        public async Task<ReviewReadDto?> GetByIdAsync(int id)
        {
            var review = await _reviewRepo.GetByIdAsync(id);
            return review == null ? null : _mapper.Map<ReviewReadDto>(review);
        }
        private async Task<string?> ValidateReviewAsync(ReviewWriteDto dto)
        {
            if (!await _itemRepo.IsExistAsync(dto.ItemId))
            {
                return "Item not found";
            }

            if(!await _customerRepo.IsExistAsync(dto.CustomerId))
            {
                return "Customer not found";
            }

            return null;
        }
        public async Task<ServiceResult<ReviewReadDto?>> AddAsync(ReviewWriteDto dto)
        {
            var validation = await ValidateReviewAsync(dto);
            if(validation != null)
            {
                return ServiceResult<ReviewReadDto?>.Fail(validation);
            }

            if (await _reviewRepo.IsForeignKeysRepeated(dto.CustomerId, dto.ItemId))
            {
                return ServiceResult<ReviewReadDto?>.Fail("You already had a review");
            }

            var review = _mapper.Map<Review>(dto);

            await _reviewRepo.AddAsync(review);
            await _reviewRepo.SaveAsync();

            return ServiceResult<ReviewReadDto?>.Ok(_mapper.Map<ReviewReadDto>(review));
        }

        public async Task<ServiceResult<ReviewReadDto?>> UpdateAsync(int id, ReviewWriteDto dto)
        {
            var review = await _reviewRepo.GetByIdAsync(id);
            if (review == null)
            {
                return ServiceResult<ReviewReadDto?>.Fail($"Review not found");
            }

            // التحقق أن المراجعة تعود لنفس العميل
            if (review.CustomerId != dto.CustomerId)
            {
                return ServiceResult<ReviewReadDto?>.Fail("You cannot update someone else's review");
            }

            var validation = await ValidateReviewAsync(dto);
            if (validation != null)
            {
                return ServiceResult<ReviewReadDto?>.Fail(validation);
            }

            _mapper.Map(dto, review);

            _reviewRepo.Update(review);
            await _reviewRepo.SaveAsync();

            return ServiceResult<ReviewReadDto?>.Ok(_mapper.Map<ReviewReadDto>(review));
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _reviewRepo.GetByIdAsync(id);
            if(review == null)
            {
                return false;
            }

            _reviewRepo.Delete(review);
            await _reviewRepo.SaveAsync();

            return true;
        }

    }
}
