using System;
using System.Collections.Generic;
using System.Text;

namespace TomadaStore.Models.DTOs.Payment
{
    public class PaymentRequestDTO
    {
        public int CustomerId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string SaleId { get; set; }
    }
}
