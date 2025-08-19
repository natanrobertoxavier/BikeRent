using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BikeDeliveryMan.Domain.Entities;

public abstract class BaseEntity
{
    public BaseEntity(
        Guid id,
        DateTime registrationDate)
    {
        Id = id;
        RegistrationDate = registrationDate;
    }

    public BaseEntity(
        Guid id)
    {
        Id = id;
    }

    public BaseEntity() { }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}
