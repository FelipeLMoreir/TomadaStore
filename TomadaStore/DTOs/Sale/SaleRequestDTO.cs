using System;
using System.Collections.Generic;
using System.Text;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleRequestDTO
    {
        public int CustomerId { get; init; } = 0;
        public List<SaleProductRequestDTO> Items { get; init; } = new();
    }
}
