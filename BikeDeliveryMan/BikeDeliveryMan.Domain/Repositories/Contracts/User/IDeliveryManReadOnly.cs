using BikeDeliveryMan.Domain.Entities;

namespace BikeDeliveryMan.Domain.Repositories.Contracts.User;

public interface IDeliveryManReadOnly
{
    Task<DeliveryMan> RecoverByEmailAsync(string email);
    Task<DeliveryMan> RecoverByCNHAsync(string cnh);
    Task<DeliveryMan> RecoverByCNPJAsync(string cnpj);
    Task<DeliveryMan> RecoverByEmailPasswordAsync(string email, string password);
}
