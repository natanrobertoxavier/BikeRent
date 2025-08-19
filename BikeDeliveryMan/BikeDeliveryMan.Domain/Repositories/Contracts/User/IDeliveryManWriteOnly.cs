using BikeDeliveryMan.Domain.Entities;

namespace BikeDeliveryMan.Domain.Repositories.Contracts.User;

public interface IDeliveryManWriteOnly
{
    Task CreateAsync(DeliveryMan deliveryMan);
}
