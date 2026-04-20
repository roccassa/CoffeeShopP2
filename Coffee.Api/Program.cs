
using Dapper;
using Dapper.Contrib.Extensions;
using Coffee.Api.DataAccess;
using Coffee.Api.DataAccess.Interfaces;
using Coffee.Api.Repositories;
using Coffee.Api.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRO DE SERVICIOS
builder.Services.AddControllers(); // ¡IMPORTANTE! Sin esto no sirven los controladores de Alex
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); // Necesario para Swagger clásico
builder.Services.AddSwaggerGen();           // Genera la documentación de Swagger

// Tus inyecciones de dependencia
builder.Services.AddScoped<IDbContext, DbContext>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();