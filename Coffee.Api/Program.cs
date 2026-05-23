
using Dapper;
using Dapper.Contrib.Extensions;
using Coffee.Api.DataAccess;
using Coffee.Api.DataAccess.Interfaces;
using Coffee.Api.Repositories;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Api.Services;
using Coffee.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// REGISTRO DE SERVICIOS
builder.Services.AddControllers(); //sin esto no sirven los controladores 
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();         

// inyecciones de dependencia
builder.Services.AddScoped<IDbContext, DbContext>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

// ── SEED: garantizar que siempre exista el usuario admin ──────────────────
// Garantiza que admin/1234 siempre exista con un hash BCrypt válido.
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<IDbContext>();

    // Asegurarse de que existan roles (necesario antes de crear usuario)
    var rolCount = await db.Connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Roles");
    if (rolCount == 0)
    {
        await db.Connection.ExecuteAsync(
            "INSERT INTO Roles (nombre) VALUES ('Administrador'),('Barista'),('Cajero')");
        Console.WriteLine("✅  Roles creados automáticamente.");
    }

    // Buscar si existe el usuario 'admin'
    var adminExists = await db.Connection.ExecuteScalarAsync<int>(
        "SELECT COUNT(*) FROM Usuarios WHERE username = 'admin'");

    if (adminExists == 0)
    {
        // No existe admin — crearlo con hash BCrypt de '1234'
        var hash = BCrypt.Net.BCrypt.HashPassword("1234");
        await db.Connection.ExecuteAsync(
            @"INSERT INTO Usuarios (rol_id, nombre_completo, username, password_hash)
              VALUES (1, 'Administrador del Sistema', 'admin', @Hash)",
            new { Hash = hash });
        Console.WriteLine("⚠️  Usuario admin no existía — se creó admin/1234 automáticamente.");
    }
    else
    {
        // Existe admin — verificar que su hash sea BCrypt válido; si no, resetearlo
        var currentHash = await db.Connection.ExecuteScalarAsync<string>(
            "SELECT password_hash FROM Usuarios WHERE username = 'admin'");

        bool hashOk = false;
        if (!string.IsNullOrEmpty(currentHash) && currentHash.StartsWith("$2"))
        {
            try { hashOk = BCrypt.Net.BCrypt.Verify("1234", currentHash); } catch { hashOk = false; }
        }

        if (!hashOk)
        {
            var newHash = BCrypt.Net.BCrypt.HashPassword("1234");
            await db.Connection.ExecuteAsync(
                "UPDATE Usuarios SET password_hash = @Hash WHERE username = 'admin'",
                new { Hash = newHash });
            Console.WriteLine("⚠️  Hash del admin estaba corrupto — se reseteó la contraseña a '1234'.");
        }
        else
        {
            Console.WriteLine("✅  Usuario admin verificado correctamente.");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"⚠️  Seed de usuarios falló: {ex.Message}");
}
// ─────────────────────────────────────────────────────────────────────────────

// Manejo global de excepciones — siempre devuelve JSON, nunca HTML
app.UseExceptionHandler(errApp => errApp.Run(async ctx =>
{
    ctx.Response.StatusCode  = 500;
    ctx.Response.ContentType = "application/json";
    var feature = ctx.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
    var msg = feature?.Error?.Message ?? "Error interno del servidor";
    await ctx.Response.WriteAsJsonAsync(new { success = false, message = msg, errors = new[] { msg } });
}));

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();
app.Run();