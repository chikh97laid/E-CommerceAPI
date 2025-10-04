using AutoMapper;
using OnlineStore.Dtos;
using OnlineStore.Dtos.Payment;
using OnlineStore.Migrations;
using OnlineStore.Models;
using OnlineStore.Repository.Interfaces;
using OnlineStore.Services.Interfaces;
using OnlineStore.Services.Results;

namespace OnlineStore.Services.Implementations
{
    public class PaymentService : IConfirmPayment 
    {
        private readonly IPaymentrepo _paymentRepo;
        private readonly IRepo<Order> _orderRepo;
        private readonly IRepo<Item> _itemRepo;
        private readonly IMapper _mapper;
        public PaymentService(IPaymentrepo paymentRepo, IMapper mapper, IRepo<Order> orderRepo, IRepo<Item> itemRepo)
        {
            _paymentRepo = paymentRepo;
            _mapper = mapper;
            _orderRepo = orderRepo;
            _itemRepo = itemRepo;
        }

        public async Task<IEnumerable<PaymentReadDto>?> GetAllAsync()
        {
            var payments = await _paymentRepo.GetAllAsync();
            return payments == null ? null : _mapper.Map<IEnumerable<PaymentReadDto>>(payments);
        }

        public async Task<PaymentReadDto?> GetByIdAsync(int id)
        {
            var payment = await _paymentRepo.GetByIdAsync(id);
            return payment == null ? null : _mapper.Map<PaymentReadDto>(payment);
        }

        private string? ValidateOrder(Order? order)
        {
            if (order == null)
                return "Order not found";

            if (order.OrderStatus == enOrderStatus.Cancelled)
                return "Cannot create payment for a cancelled order";

            return null; 
        }

        public async Task<ServiceResult<PaymentReadDto?>> AddAsync(PaymentWriteDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(dto.OrderId);

            var validate = ValidateOrder(order);
            if(validate != null)
            {
                return ServiceResult<PaymentReadDto?>.Fail(validate);
            }

            var payment = _mapper.Map<Payment>(dto);
            payment.Amount = order!.OrderItems.Sum(oi => oi.Price * oi.Quentity);
            payment.PaymentStatus = enPaymentStatus.Pending;

            await _paymentRepo.AddAsync(payment);
            await _paymentRepo.SaveAsync();

            var paymentReadDto = _mapper.Map<PaymentReadDto>(payment);

            return ServiceResult<PaymentReadDto?>.Ok(paymentReadDto);
        }

        private string? IsFound(Payment? payment, Order? order)
        {
            if (payment == null)
            {
                return $"Payment not found";
            }

            if(order == null)
            {
                return $"Order not found";
            }

            return null;
        }

        public async Task<ServiceResult<PaymentReadDto?>> UpdateAsync(int id, PaymentWriteDto dto)
        {
            var payment = await _paymentRepo.GetByIdAsync(id);
            var order = await _orderRepo.GetByIdAsync(dto.OrderId);

            //if (payment?.OrderId != dto.OrderId)
            //{
            //    return ServiceResult<PaymentReadDto?>.Fail("You cannot update someone else's payment");
            //}

            var result = IsFound(payment, order);
            if(result !=  null)
            {
                return ServiceResult<PaymentReadDto?>.Fail(result);
            }

            _mapper.Map(dto, payment);

            payment!.Amount = order!.OrderItems.Sum(oi => oi.Price * oi.Quentity);

            _paymentRepo.Update(payment);
            await _paymentRepo.SaveAsync();

            var paymentReadDto = _mapper.Map<PaymentReadDto>(payment);

            return ServiceResult<PaymentReadDto?>.Ok(paymentReadDto);
        }

        private string? ValidatePayment(Payment? payment)
        {
            if (payment == null)
            {
                return "Payment not found";
            }

            if (payment.PaymentStatus != enPaymentStatus.Pending)
            {
                return "Payment already processed";
            }

            return null;
        }

        private string? ValidateItem(Item? item, OrderItem? orderItem)
        {
            if(item == null)
            {
                return "Item not found";
            }

            if(orderItem == null)
            {
                return "OrderItem not found";
            }

            if (orderItem.Quentity > item.Quantity)
            {
                return "Insufficient stock";
            }

            return null;
        }

        public async Task<ServiceResult<PaymentReadDto?>> ConfirmPaymentAsync(int id)
        {
            var payment = await _paymentRepo.GetByIdAsync(id);

            var validateResult = ValidatePayment(payment);
            if (validateResult != null)
            {
                return ServiceResult<PaymentReadDto?>.Fail(validateResult);
            }

            using var transaction = await _paymentRepo.TransactionAsync();
            try
            {
                foreach (var orderItem in payment!.Order.OrderItems)
                {
                    var item = await _itemRepo.GetByIdAsync(orderItem.ItemId);
                    var validation = ValidateItem(item, orderItem);
                    if (validation != null)
                    {
                        return ServiceResult<PaymentReadDto?>.Fail(validation);
                    }

                    item!.Quantity -= orderItem.Quentity;
                }

                payment.PaymentStatus = enPaymentStatus.Paid;

                await _paymentRepo.SaveAsync();
                await transaction.CommitAsync();

                return ServiceResult<PaymentReadDto?>.Ok(_mapper.Map<PaymentReadDto>(payment));

            }
            catch(Exception ex) 
            {
                await transaction.RollbackAsync();
                payment!.PaymentStatus = enPaymentStatus.Failed;
                await _paymentRepo.SaveAsync();
                return ServiceResult<PaymentReadDto?>.Fail("Unexpected error: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await _paymentRepo.GetByIdAsync(id);
            if (payment == null) return false;

            _paymentRepo.Delete(payment);
            await _paymentRepo.SaveAsync();

            return true;
        }

    }
}
