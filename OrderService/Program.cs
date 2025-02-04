using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Commands;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;
using OrderService.Domain.Interfaces;
using OrderService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Подключение базы данных PostgreSQL
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозитория заказов
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Регистрация MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Регистрация HTTP-клиента для взаимодействия с ProductService
builder.Services.AddHttpClient<IProductServiceClient, ProductServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ProductService:BaseUrl"]);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Конфигурация Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();