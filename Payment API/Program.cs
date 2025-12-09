using Payment_API.Repository.Interfaces;
using Payment_API.Services.Interfaces;
using TomadaStore.PaymentAPI.Repositories;
using TomadaStore.PaymentAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IPaymentProducerRepository, PaymentProducerRepository>();
builder.Services.AddScoped<IPaymentConsumerService, PaymentConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
