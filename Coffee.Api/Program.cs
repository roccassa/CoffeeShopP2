
using Dapper;
using Dapper.Contrib.Extensions;
using Coffee.Api.DataAccess;
using Coffee.Api.DataAccess.Interfaces;
using Coffee.Api.Repositories;
using Coffee.Api.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

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