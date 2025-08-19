using BikeDeliveryMan.Communication.Request;
using BikeDeliveryMan.Communication.Response;
using BikeDeliveryMan.Domain.Entities;

namespace BikeDeliveryMan.Application.Mapping;

public static class DeliveryManMappingDeliveryManMapping
{
    public static DeliveryMan ToEntity(this RegisterDeliveryMan request, string encryptedPassword)
    {
        return new DeliveryMan(
            Guid.NewGuid(),
            DateTime.UtcNow,
            request.Name,
            request.Email.ToLower(),
            encryptedPassword,
            request.CNPJ,
            request.BirthDate,
            request.CNHNumber,
            request.CNHCategory
        );
    }

    public static ResponseDeliveryMan ToResponse(this DeliveryMan deliveryMan)
    {
        return new ResponseDeliveryMan(
            deliveryMan.Id,
            deliveryMan.RegistrationDate,
            deliveryMan.Name,
            deliveryMan.Email,
            deliveryMan.CNPJ,
            deliveryMan.BirthDate,
            deliveryMan.CNHNumber,
            deliveryMan.CNHCategory
        );
    }

    public static ResponseLogin ToResponseLogin(this DeliveryMan deliveryMan, string token)
    {
        return new ResponseLogin(
            deliveryMan.Name,
            deliveryMan.Email,
            token
        );
    }
}