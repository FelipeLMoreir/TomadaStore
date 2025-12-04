namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleProductRequestDTO
    {
        public string ProductId { get; init; }  = string.Empty;
        public int Quantity { get; init; } = 0;
    }
}