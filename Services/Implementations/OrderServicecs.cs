using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using OnlineStore.Dtos.Order;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using OnlineStore.Services.Interfaces;
using OnlineStore.Services.Results;
using System.Collections.Generic;

namespace OnlineStore.Services.Implementations
{
    public class OrderServicecs : IOrderService
    {
        private readonly IRepo<Order> _orderRepo;
        private readonly IRepo<Customer> _customerRepo;
        private readonly IShippingRepo _shippingRepo;
        private readonly IRepo<Item> _itemRepo;
        private readonly IMapper _mapper;

        public OrderServicecs(IRepo<Order> orderRepo, IMapper mapper, IRepo<Customer> customerRepo,
                              IShippingRepo shippingRepo, IRepo<Item> itemRepo)
        { 
            _orderRepo = orderRepo;
            _mapper = mapper;
            _customerRepo = customerRepo;
            _shippingRepo = shippingRepo;
            _itemRepo = itemRepo;
        }

        public async Task<IEnumerable<OrderReadDto>?> GetAllAsync()
        {
            var orders = (await _orderRepo.GetAllAsync()).ToList();
            var readDto = _mapper.Map<IEnumerable<OrderReadDto>>(orders).ToList();

            for ( int i = 0; i < orders.Count; i++)
               {
                var order = orders[i];
                var dto = readDto[i];

                foreach (var oi in order.OrderItems)
                {
                    dto.Items.Add(new OrderItemReadDto()
                    {
                        ItemId = oi.ItemId,
                        ItemName = oi.Item.Name,
                        Quantity = oi.Quentity,
                        Price = oi.Price
                    });

                }

                dto.Total = order.OrderItems.Sum(oi => oi.Quentity * oi.Price);
            }

            return orders == null ? null : readDto;
        }

        public async Task<OrderReadDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return null;
            var readDto = _mapper.Map<OrderReadDto>(order);
            readDto.Items = order.OrderItems.Select(oi => new OrderItemReadDto()
            {
                ItemId = oi.ItemId,
                ItemName = oi.Item.Name,
                Quantity = oi.Quentity,
                Price = oi.Price
            }).ToList();
            readDto.Total = order.OrderItems.Sum(oi => oi.Quentity * oi.Price);
            return order == null ? null : readDto;
        }

        private async Task<string?> ValidateOrderDataAsync(OrderWriteDto dto)
        {

            if (!await _customerRepo.IsExistAsync(dto.CustomerId))
            {
                return "Customer not found";
            }

            if (dto.ShippingId != null && !await _shippingRepo.IsExistAsync(dto.ShippingId.Value))
            {
                return "Shipping method not found";
            }

            foreach (var itemDto in dto.Items)
            {
                if (await _itemRepo.GetByIdAsync(itemDto.ItemId) == null)
                {
                    return $"Item with Id {itemDto.ItemId} not found";
                }
            }

            return null;
        } 

        public async Task<ServiceResult<OrderReadDto?>> AddAsync(OrderWriteDto dto)
        {
            var validationError = await ValidateOrderDataAsync(dto);
            if (validationError != null)
            {
                return ServiceResult<OrderReadDto?>.Fail(validationError);
            }

            var order = _mapper.Map<Order>(dto);
            
            order.OrderStatus = enOrderStatus.Processing;

            foreach (var itemDto in dto.Items)
            {
                var item = await _itemRepo.GetByIdAsync(itemDto.ItemId);

                var orderItem = new OrderItem()
                {
                    ItemId = itemDto.ItemId,
                    Quentity = itemDto.Quantity,
                    Price = item!.Price
                };

                order.OrderItems.Add(orderItem);
            }

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveAsync();

            order = await _orderRepo.GetByIdAsync(order.Id);
            if (order == null)
            {
                return ServiceResult<OrderReadDto?>.Fail("Order not found");
            }

            var orderReaddto = _mapper.Map<OrderReadDto>(order);
            orderReaddto.Items = order.OrderItems.Select(oi => new OrderItemReadDto()
            {
                ItemId = oi.ItemId,
                ItemName = oi.Item.Name,
                Quantity = oi.Quentity,
                Price = oi.Price
            }).ToList();
            orderReaddto.Total = order.OrderItems.Sum(oi => oi.Quentity * oi.Price);

            return ServiceResult<OrderReadDto?>.Ok(orderReaddto);
        }

        public async Task<ServiceResult<OrderReadDto?>> UpdateAsync(int id, OrderWriteDto dto)
        {

            if (!await _orderRepo.IsExistAsync(id))
            {
                return ServiceResult<OrderReadDto?>.Fail("Order not found");
            }

            var validationError = await ValidateOrderDataAsync(dto);
            if (validationError != null)
            {
                return ServiceResult<OrderReadDto?>.Fail(validationError);
            }
            // if there is a double item in dto
            dto.Items = dto.Items
                .GroupBy(x => x.ItemId)
                .Select(g => new OrderItemWriteDto()  
                {
                    ItemId = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();
            
            var order = await _orderRepo.GetByIdAsync(id);

            _mapper.Map(dto, order);

            foreach (var itemDto in dto.Items)
            {
                var item = await _itemRepo.GetByIdAsync(itemDto.ItemId);

                var existingOrderItem = order!.OrderItems.SingleOrDefault(oi => oi.ItemId == itemDto.ItemId);
                if (existingOrderItem != null)
                {
                    existingOrderItem.Quentity = itemDto.Quantity;
                    existingOrderItem.Price = item!.Price;
                }
                else
                {
                    order?.OrderItems.Add(new OrderItem()
                    {
                        ItemId = itemDto.ItemId,
                        Quentity = itemDto.Quantity,
                        Price = item!.Price  // ! = null-forgiving operator
                    });
                }

            }

            _orderRepo.Update(order!);
            await _orderRepo.SaveAsync();

            order = await _orderRepo.GetByIdAsync(id);
            var orderReaddto = _mapper.Map<OrderReadDto>(order);
            orderReaddto.Items = order!.OrderItems.Select(oi => new OrderItemReadDto()
            {
                ItemId = oi.ItemId,
                ItemName = oi.Item.Name,
                Quantity = oi.Quentity,
                Price = oi.Price
            }).ToList();
            orderReaddto.Total = order.OrderItems.Sum(oi => oi.Quentity * oi.Price);

            return ServiceResult<OrderReadDto?>.Ok(orderReaddto);
        }

        public async Task<ServiceResult<OrderReadDto?>> CancelOrder(int id)
        {

            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
            {
                return ServiceResult<OrderReadDto?>.Fail("Order not found");
            }

            if (order.OrderStatus != enOrderStatus.Processing)
            {
                return ServiceResult<OrderReadDto?>.Fail("Order cannot be cancelled once shipped.");
            }

            order.OrderStatus = enOrderStatus.Cancelled;
            await _orderRepo.SaveAsync();

            var orderReaddto = _mapper.Map<OrderReadDto>(order);
            orderReaddto.Items = order!.OrderItems.Select(oi => new OrderItemReadDto()
            {
                ItemId = oi.ItemId,
                ItemName = oi.Item.Name,
                Quantity = oi.Quentity,
                Price = oi.Price
            }).ToList();

            orderReaddto.Total = order.OrderItems.Sum(oi => oi.Quentity * oi.Price);

            return ServiceResult<OrderReadDto?>.Ok(orderReaddto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null) return false;

            _orderRepo.Delete(order);
            await _orderRepo.SaveAsync();
            return true;
        }

    }
}
