using BikeDeliveryMan.Domain.Entities;

namespace BikeDeliveryMan.Application.Services;

public interface ILoggedUser
{
    Task<DeliveryMan> GetLoggedUserAsync();
}
