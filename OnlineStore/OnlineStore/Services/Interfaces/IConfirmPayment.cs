using OnlineStore.Dtos.Payment;
using OnlineStore.Services.Results;

namespace OnlineStore.Services.Interfaces
{
    public interface IConfirmPayment : IService<PaymentReadDto, PaymentWriteDto>
    {
        Task<ServiceResult<PaymentReadDto?>> ConfirmPaymentAsync(int id);
    }
}
