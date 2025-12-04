using MongoDB.Bson;
using TomadaStore.Models.DTOs.Product;

namespace TomadaStore.ProductAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task CreateProductAsync(ProductRequestDTO productDTO);
        Task<List<ProductResponseDTO>> GetAllProductsAsync();
        Task<ProductResponseDTO> GetProductByIDAsync(ObjectId id);
    }
}