using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Category;

namespace TomadaStore.Models.Models
{
    public static class SaleExtensions
    {
        public static SaleResponseDTO ToResponseDTO(this Sale sale)
        {
            return new SaleResponseDTO
            {
                Id = sale.Id,
                Customer = new CustomerResponseDTO
                {
                    Id = sale.Customer.Id,
                    FirstName = sale.Customer.FirstName,
                    LastName = sale.Customer.LastName,
                    Email = sale.Customer.Email,
                    PhoneNumber = sale.Customer.PhoneNumber
                },
                Products = sale.Products.Select(p => new ProductResponseDTO
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Category = new CategoryResponseDTO
                    {
                        Id = p.Category.Id.ToString(),
                        Name = p.Category.Name,
                        Description = p.Category.Description
                    }
                }).ToList(),
                SaleDate = sale.SaleDate,
                TotalPrice = sale.TotalPrice
            };
        }
    }
}
