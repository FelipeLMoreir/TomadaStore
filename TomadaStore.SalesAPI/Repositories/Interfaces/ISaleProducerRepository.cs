namespace TomadaStore.SalesAPI.Repositories.Interfaces
{
    public interface ISaleProducerRepository
    {
        Task PublishSaleAsync(object saleEvent);
    }
}
