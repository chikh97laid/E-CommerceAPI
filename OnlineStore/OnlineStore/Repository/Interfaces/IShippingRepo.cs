using OnlineStore.Models;

namespace OnlineStore.Repository.Interfaces
{
    public interface IShippingRepo : IRepo<Shipping>
    {
        void UpdateShippingStatus(Shipping shipping);
    }
}
