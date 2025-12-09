using System;
using System.Collections.Generic;
using System.Text;
using TomadaStore.Models.DTOs.Sale;

namespace TomadaStore.Models.DTOs.Sales
{
    public class SaleEventDTO  
    {
        public string SaleId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<SaleProductItemDTO> Products { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
