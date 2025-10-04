using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Extensions;
using OnlineStore.Models;
using OnlineStore.Profiles;
using System.Reflection;
using AutoMapper;
using OnlineStore.Dtos.Category;
using OnlineStore.Services.Implementations;
using OnlineStore.Services.Interfaces;
using OnlineStore.Dtos.Review;
using System.Text.Json.Serialization;
using OnlineStore.Dtos.Customer;
using OnlineStore.Dtos.Shipping;
using OnlineStore.Repository.Interfaces;
using OnlineStore.Repository.Implementations;

var builder = WebApplication.CreateBuilder(args);

// أنشئ Configuration
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<AllProfiles>();  // يمكنك إضافة أكثر من Profile هنا
});

//  أنشئ الـMapper
IMapper mapper = mapperConfig.CreateMapper();

//  سجل الـMapper في الـDI Container
builder.Services.AddSingleton(mapper);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("myCon"))
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging()
);

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

//repos scope
builder.Services.AddScoped<IRepo<Category>, CategoryRepo>();
builder.Services.AddScoped<IRepo<Item>, ItemRepo>();
builder.Services.AddScoped<IRepo<Customer>, CustomerRepo>();
builder.Services.AddScoped<IRepo<Order>, OrderRepo>();
builder.Services.AddScoped<IShippingRepo, ShippingRepo>();
builder.Services.AddScoped<IPaymentrepo, PaymentRepo>();
builder.Services.AddScoped<IReviewRepo, ReviewRepo>();

// services scope
builder.Services.AddScoped<IService<CategoryReadDto, CategoryWriteDto>, CategoryService>();
builder.Services.AddScoped<IService<CustomerReadDto, CustomerWriteDto>, CustomerService>();
builder.Services.AddScoped<IitemService, ItemService>();
builder.Services.AddScoped<IOrderService, OrderServicecs>();
builder.Services.AddScoped<IConfirmPayment, PaymentService>();
builder.Services.AddScoped<IService<ReviewReadDto, ReviewWriteDto>, ReviewService>();
builder.Services.AddScoped<IShippingService, ShippingService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddControllers()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenJWTAuth();

builder.Services.AddCustomJWTAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
