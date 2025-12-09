using System;
using System.Collections.Generic;
using System.Text;

namespace TomadaStore.Models.Models
{
    public class Payment
    {
        public string Id { get; private set; }
        public string SaleId { get; private set; }
        public string Status { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime ProcessedAt { get; private set; }

        public Payment(string saleId, string status, decimal amount)
        {
            Id = Guid.NewGuid().ToString();
            SaleId = saleId;
            Status = status;
            Amount = amount;
            ProcessedAt = DateTime.UtcNow;
        }
    }
}
