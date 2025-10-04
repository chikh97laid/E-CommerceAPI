using AutoMapper;
using OnlineStore.Dtos.Customer;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using OnlineStore.Services.Interfaces;
using OnlineStore.Services.Results;

namespace OnlineStore.Services.Implementations
{
    public class CustomerService : IService<CustomerReadDto, CustomerWriteDto>
    {
        private readonly IRepo<Customer> _customerRepo;
        private readonly IMapper _mapper;

        public CustomerService(IRepo<Customer> customerRepo, IMapper mapper)
        {
            _customerRepo = customerRepo;
            _mapper = mapper;
        }


        public async Task<IEnumerable<CustomerReadDto>?> GetAllAsync()
        {
            var customers = await _customerRepo.GetAllAsync();
            return customers == null ? null : _mapper.Map<IEnumerable<CustomerReadDto>>(customers);
        }

        public async Task<CustomerReadDto?> GetByIdAsync(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            return customer == null ? null : _mapper.Map<CustomerReadDto>(customer);
        }

        public async Task<ServiceResult<CustomerReadDto?>> AddAsync(CustomerWriteDto dtCustomer)
        {
            var customer = _mapper.Map<Customer>(dtCustomer);

            await _customerRepo.AddAsync(customer);
            await _customerRepo.SaveAsync();

            var customerReadDto = _mapper.Map<CustomerReadDto>(customer);

            return ServiceResult<CustomerReadDto?>.Ok(customerReadDto);
        }

        public async Task<ServiceResult<CustomerReadDto?>> UpdateAsync(int id, CustomerWriteDto dto)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            
            if(customer == null) return ServiceResult<CustomerReadDto?>.Fail("Customer not found");
           
            _mapper.Map(dto, customer);

            _customerRepo.Update(customer);
            await _customerRepo.SaveAsync();

            var customerReadDto = _mapper.Map<CustomerReadDto>(customer);

            return ServiceResult<CustomerReadDto?>.Ok(customerReadDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null) return false;
            
            _customerRepo.Delete(customer);
            await _customerRepo.SaveAsync();
            
            return true;    
        }

    }
}
