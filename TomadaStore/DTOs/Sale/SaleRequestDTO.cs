using System;
using System.Collections.Generic;
using System.Text;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleRequestDTO
    {
        public int CustomerId { get; set; }
        public List<SaleProductItemDTO> Products { get; set; } = new();
    }
}
