using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Extensions;
using OnlineStore.Models;
using OnlineStore.Profiles;
using AutoMapper;
using OnlineStore.Dtos.Category;
using OnlineStore.Services.Implementations;
using OnlineStore.Services.Interfaces;
using OnlineStore.Dtos.Review;
using OnlineStore.Dtos.Customer;
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

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

// Configure the HTTP request pipeline.
// اترك هذه الدالتين بدون شرط ليفتح السويغر على السيرفر الخارجي
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Store API V1");
    // هذا السطر اختياري: يجعل السويغر يفتح مباشرة بمجرد كتابة رابط الموقع بدون الحاجة لكتابة /swagger
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// --- كود تهيئة قاعدة البيانات وإنشاء الـ Admin والرول تلقائياً ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // 1. تطبيق الـ Migrations تلقائياً
        context.Database.Migrate();

        // جلب مدير الصلاحيات ومدير المستخدمين من الـ Dependency Injection
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();

        // 2. إنشاء رول Admin إذا لم تكن موجودة
        string roleName = "Admin";
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        // 3. إنشاء مستخدم Admin إذا لم يكن موجوداً
        string adminEmail = "admin@store.com"; // يمكنك تغيير الإيميل هنا
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var newAdmin = new AppUser
            {
                UserName = "Admin",
                Email = adminEmail,
                EmailConfirmed = true
                // أضف أي حقول إضافية مخصصة موجودة لديك في كلاس AppUser هنا (مثل FirstName أو LastName)
            };

            // ضع كلمة مرور قوية تتوافق مع شروط Identity (أحرف كبيرة، صغيرة، أرقام، ورمز)
            string adminPassword = "Admin@Password123";

            var createAdminResult = await userManager.CreateAsync(newAdmin, adminPassword);

            // 4. ربط المستخدم برول الـ Admin بعد نجاح إنشائه
            if (createAdminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, roleName);
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "حدث خطأ أثناء تهيئة قاعدة البيانات أو إنشاء حساب الـ Admin.");
    }
}

var port = Environment.GetEnvironmentVariable("PORT");

if (!string.IsNullOrEmpty(port))
{
    app.Urls.Add($"http://0.0.0.0:{port}");
}

app.Run(); // السطر الأخير الثابت في مشروعك