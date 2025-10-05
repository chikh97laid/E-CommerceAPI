using AutoMapper;
using OnlineStore.Dtos.Category;
using OnlineStore.Dtos.Customer;
using OnlineStore.Dtos.Item;
using OnlineStore.Dtos.Order;
using OnlineStore.Dtos.Payment;
using OnlineStore.Dtos.Review;
using OnlineStore.Dtos.Shipping;
using OnlineStore.Models;

namespace OnlineStore.Profiles
{
    public class AllProfiles : Profile
    {
        public AllProfiles()
        {
            // category --> categoryReadDto
            CreateMap<Category, CategoryReadDto>();

            // categorywriteDto --> category 
            CreateMap<CategoryWriteDto, Category>();

            CreateMap<Customer, CustomerReadDto>();
            CreateMap<CustomerWriteDto, Customer>();

            CreateMap<Item, ItemReadDto>().ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => $"/api/items/{src.Id}/image"));
            CreateMap<ItemWriteDto, Item>().ForMember(des => des.Image, opt => opt.Ignore());

            CreateMap<Order, OrderReadDto>();
            CreateMap<OrderWriteDto, Order>();

            CreateMap<Payment, PaymentReadDto>();
            CreateMap<PaymentWriteDto, Payment>();

            CreateMap<Review, ReviewReadDto>();
            CreateMap<ReviewWriteDto, Review>();

            CreateMap<Shipping, ShippingReadDto>();
            CreateMap<ShippingWriteDto, Shipping>();

        }
    }
}
