using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SonicStore.Business.Service.AccountService;
using SonicStore.Business.Service.CartService;
using SonicStore.Business.Service.CheckoutService;
using SonicStore.Business.Service.OrderService;
using SonicStore.Business.Service.ProductService;
using SonicStore.Business.Service.PromotionService;
using SonicStore.Business.Service.VnPayService;
using SonicStore.Common.Utils;
using SonicStore.Repository.Repository.AccountRepo;
using SonicStore.Repository.Repository.CartRepo;
using SonicStore.Repository.Repository.CheckoutRepo;
using SonicStore.Repository.Repository.InventoryRepo;
using SonicStore.Repository.Repository.OrderRepo;
using SonicStore.Repository.Repository.ProductRepo;
using SonicStore.Repository.Repository.PromotionRepo;
using SonicStore.Repository.Repository.UserAddressRepo;
using SonicStore.Repository.Repository.UserRepo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(option =>
{
    option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/SonicStore/Login/Index";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.LoginPath = "/SonicStore/Login/login";
    options.AccessDeniedPath = "/error";
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});

builder.Services.AddDbContext<SonicStore.Repository.Entity.SonicStoreContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("SonicStore")!;
    options.UseSqlServer(connectionString);
});
// Đăng ký các Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
// Đăng ký các Service
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderListService, OrderListService  >();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
// Đăng ký các Utility
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<EncriptPassword>();
builder.Services.AddSingleton<IVnPayService, VnPayService>();
builder.Services.AddScoped<ICheckoutRepository, CheckoutRepository>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>(); // Added this line
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.Cookie.Name = "session";
    option.IdleTimeout = new TimeSpan(0, 50, 0);
});
builder.Services.AddSingleton<IVnPayService, VnPayService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/home");
        return;
    }
    await next();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "SonicStore",
        areaName: "SonicStore",
        pattern: "SonicStore/{controller=Home}/{action=HomeScreen}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "register-google",
        pattern: "SonicStore/Login/RegisterGoogle",
        defaults: new { controller = "Login", action = "RegisterGoogle" });

    endpoints.MapControllerRoute(
        name: "google-response",
        pattern: "SonicStore/Login/GoogleResponse",
        defaults: new { controller = "Login", action = "GoogleResponse" });

    endpoints.MapControllerRoute(
        name: "notice-success",
        pattern: "SonicStore/Login/NoticeSuccess",
        defaults: new { controller = "Login", action = "NoticeSuccess" });
});

app.Run();