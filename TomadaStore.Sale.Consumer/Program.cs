using TomadaStore.Sale.Consumer.Repository;
using TomadaStore.Sale.Consumer.Repository.Interfaces;
using TomadaStore.Sale.Consumer.Service;
using TomadaStore.Sale.Consumer.Service.Interfaces;
using TomadaStore.SaleAPI.Repository;
using TomadaStore.SaleAPI.Repository.Interfaces;
using TomadaStore.SalesAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<ConnectionDB>();

builder.Services.AddScoped<ISaleConsumerRepository, SaleConsumerRepository>();

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleConsumerService, SaleConsumerService>();

builder.Services.AddHttpClient("PaymentAPI", client =>
    client.BaseAddress = new Uri("https://localhost:7162")); 



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
