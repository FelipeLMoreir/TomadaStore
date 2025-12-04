]using TomadaStore.Models.DTOs.Product;

namespace TomadaStore.ProductAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetProductAsync();
        Task<ProductResponseDTO> GetProductByIDAsync(string id);
        Task CreateProductAsync(ProductRequestDTO product);
        Task UpdateProductAsync(string id, ProductRequestDTO productDto);
        Task DeleteProductAsync(string id);
    }
}
