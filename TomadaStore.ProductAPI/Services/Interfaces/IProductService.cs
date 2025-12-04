using MongoDB.Bson;
using TomadaStore.Models.DTOs.Product;

namespace TomadaStore.ProductAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetProductAsync();
        Task<ProductResponseDTO> GetProductByIDAsync(ObjectId id);
        Task CreateProductAsync(ProductRequestDTO product);
        Task UpdateProductAsync(ObjectId id, ProductRequestDTO productDto);
        Task DeleteProductAsync(ObjectId id);
    }
}
