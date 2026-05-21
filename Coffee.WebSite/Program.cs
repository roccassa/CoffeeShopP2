using Coffee.WebSite.Services;

using Coffee.WebSite.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

//para login
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuración del HttpClient Base
builder.Services.AddHttpClient("ApiClient", client => {
        client.BaseAddress = new Uri("http://localhost:5140/");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.Timeout = TimeSpan.FromSeconds(30);
})
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true 
    });

// === REGISTRO CORRECTO DE SERVICIOS APUNTANDO AL CLIENTE CONFIGURADO ===
builder.Services.AddScoped<ICategoryService>(sp =>
    new CategoryService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));

builder.Services.AddScoped<ICustomerService>(sp =>
    new CustomerService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));

builder.Services.AddScoped<IRoleService>(sp => 
    new RoleService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));

builder.Services.AddScoped<IProductService>(sp =>
    new ProductService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));

builder.Services.AddScoped<IProductVariantService>(sp =>
    new ProductVariantService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));

builder.Services.AddScoped<IPaymentMethodService>(sp =>
    new PaymentMethodService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));

builder.Services.AddScoped<IOrderService>(sp =>
    new OrderService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));

builder.Services.AddScoped<IOrderDetailService>(sp =>
    new OrderDetailService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));

builder.Services.AddScoped<IUserService>(sp =>
    new UserService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")));

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error"); 
    app.UseHsts(); 
}

//app.UseHttpsRedirection();
app.UseStaticFiles();//
app.UseRouting();
app.UseSession(); // para login y autenticar
app.UseAuthorization();
app.MapStaticAssets();
//app.MapRazorPages().WithStaticAssets();
app.MapRazorPages();

// === NUEVO: Redirigir la raíz (/) al Login ===
app.MapGet("/", () => Results.Redirect("/Account/Login"));
app.Run();