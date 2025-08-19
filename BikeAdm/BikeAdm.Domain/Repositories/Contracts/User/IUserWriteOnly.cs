namespace BikeAdm.Domain.Repositories.Contracts.User;

public interface IUserWriteOnly
{
    Task CreateAsync(Entities.User user);
}
