using System;
using System.Collections.Generic;
using System.Text;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleResponseDTO
    {
        public string Id { get; set; }
        public CustomerResponseDTO Customer { get; set; }
        public List<ProductResponseDTO> Products { get; set; } = new();
        public DateTime SaleDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
