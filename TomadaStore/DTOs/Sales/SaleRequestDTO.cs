using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleRequestDTO
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public string ProductId { get; set; }
        public List<SaleProductItemDTO> Products { get; set; } = new();
    }
}
