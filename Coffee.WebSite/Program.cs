using Coffee.WebSite.Services;
using Coffee.WebSite.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

//para login
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient("ApiClient", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5140/"); // O cambia a https si es necesario
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.Timeout = TimeSpan.FromSeconds(30);
    })
// Agrega esta sección para ignorar el error de certificado inválido:
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    });

builder.Services.AddHttpClient<ICategoryService, CategoryService>("ApiClient");
builder.Services.AddHttpClient<ICustomerService, CustomerService>("ApiClient");
builder.Services.AddHttpClient<IRoleService, RoleService>("ApiClient");
builder.Services.AddHttpClient<IProductService, ProductService>("ApiClient");
builder.Services.AddHttpClient<IProductVariantService, ProductVariantService>("ApiClient");
builder.Services.AddHttpClient<IPaymentMethodService, PaymentMethodService>("ApiClient");
builder.Services.AddHttpClient<IOrderService, OrderService>("ApiClient");
builder.Services.AddHttpClient<IOrderDetailService, OrderDetailService>("ApiClient");
builder.Services.AddHttpClient<IUserService, UserService>("ApiClient");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();//
app.UseRouting();
app.UseSession();           // para login y autenticar
app.UseAuthorization();
app.MapStaticAssets();
//app.MapRazorPages().WithStaticAssets();
app.MapRazorPages();

// === NUEVO: Redirigir la raíz (/) al Login ===
app.MapGet("/", () => Results.Redirect("/Account/Login"));

app.Run();
