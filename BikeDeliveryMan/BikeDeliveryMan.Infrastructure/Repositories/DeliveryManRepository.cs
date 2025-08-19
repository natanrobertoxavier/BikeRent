using BikeDeliveryMan.Domain.Entities;
using BikeDeliveryMan.Domain.Repositories.Contracts.User;
using MongoDB.Driver;

namespace BikeDeliveryMan.Infrastructure.Repositories;

public class DeliveryManRepository(IMongoCollection<DeliveryMan> collection) : IDeliveryManReadOnly, IDeliveryManWriteOnly
{
    private readonly IMongoCollection<DeliveryMan> _collection = collection;

    public async Task CreateAsync(DeliveryMan deliveryMan) =>
        await _collection.InsertOneAsync(deliveryMan);

    public async Task<DeliveryMan> RecoverByCNHAsync(string cnh) =>
        await _collection.Find(x => x.CNHNumber == cnh).FirstOrDefaultAsync();

    public async Task<DeliveryMan> RecoverByCNPJAsync(string cnpj) =>
        await _collection.Find(x => x.CNPJ == cnpj).FirstOrDefaultAsync();

    public async Task<DeliveryMan> RecoverByEmailAsync(string email) =>
        await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();

    public async Task<DeliveryMan> RecoverByEmailPasswordAsync(string email, string password) =>
        await _collection.Find(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
}
