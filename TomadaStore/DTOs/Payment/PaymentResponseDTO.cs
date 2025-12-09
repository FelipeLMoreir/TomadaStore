using System;
using System.Collections.Generic;
using System.Text;

namespace TomadaStore.Models.DTOs.Payment
{
    public class PaymentResponseDTO
    {
        public string PaymentId { get; set; }
        public string SaleId { get; set; }
        public string Status { get; set; } 
        public decimal Amount { get; set; }
    }
}
