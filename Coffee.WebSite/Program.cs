using Coffee.WebSite.Services;
using Coffee.WebSite.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddHttpClient<ICategoryService, CategoryService>();
builder.Services.AddHttpClient<ICustomerService, CustomerService>();
builder.Services.AddHttpClient<IRoleService, RoleService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IProductVariantService, ProductVariantService>();
builder.Services.AddHttpClient<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddHttpClient<IOrderService, OrderService>();
builder.Services.AddHttpClient<IOrderDetailService, OrderDetailService>();
builder.Services.AddHttpClient<IUserService, UserService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
