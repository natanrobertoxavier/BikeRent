using BikeAdm.Domain.Repositories.Contracts.User;
using MongoDB.Driver;

namespace BikeAdm.Infrastructure.Repositories.User;

public class UserRepository(IMongoCollection<Domain.Entities.User> collection) : IUserReadOnly, IUserWriteOnly
{
    private readonly IMongoCollection<Domain.Entities.User> _collection = collection;

    public async Task CreateAsync(Domain.Entities.User user)
    {
        await _collection.InsertOneAsync(user);
    }

    public async Task<Domain.Entities.User> RecoverByEmailAsync(string email) =>
        await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();

    public async Task<Domain.Entities.User> RecoverByEmailPasswordAsync(string email, string password) =>
        await _collection.Find(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
}