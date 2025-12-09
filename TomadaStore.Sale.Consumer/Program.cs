using TomadaStore.Sale.Consumer.Service;
using TomadaStore.Sale.Consumer.Service.Interfaces;
using TomadaStore.SaleAPI.Repository;
using TomadaStore.SaleAPI.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ISaleConsumerService, SaleConsumerService>();
builder.Services.AddHttpClient("PaymentAPI", client =>
    client.BaseAddress = new Uri("https://localhost:7162")); 

builder.Services.AddScoped<ISaleConsumerService, SaleConsumerService>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleConsumerService, SaleConsumerService>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
