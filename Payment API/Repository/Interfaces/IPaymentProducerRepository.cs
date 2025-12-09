using TomadaStore.Models.DTOs.Payment;

namespace Payment_API.Repository.Interfaces
{
    public interface IPaymentProducerRepository
    {
        Task PublishPaymentConfirmedAsync(PaymentResponseDTO payment);
    }
}
