using TomadaStore.SalesAPI.Data;
using TomadaStore.SalesAPI.Repositories;
using TomadaStore.SalesAPI.Repositories.Interfaces;
using TomadaStore.SalesAPI.Services;
using TomadaStore.SalesAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<ConnectionDB>();

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();

builder.Services.AddHttpClient<ISaleService, SaleService>(client =>
        client.BaseAddress = new Uri("https://localhost:5001/api/v1/Customer"));

builder.Services.AddHttpClient<ISaleService, SaleService>(client =>
        client.BaseAddress = new Uri("https://localhost:6001/api/v1/Product"));


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
