using ShopOnline_API.Data;
using Microsoft.EntityFrameworkCore;
using ShopOnline_API.Repositories.Contracts;
using ShopOnline_API.Repositories;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ShopOnlineDbContext>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();


var app = builder.Build();

app.UseCors(policy => policy.WithOrigins("http://localhost:7063", "https://localhost:7063")
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType)
    );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
