namespace BikeAdm.Domain.Repositories.Contracts.User;

public interface IUserReadOnly
{
    Task<Entities.User> RecoverByEmailAsync(string email);
    Task<Entities.User> RecoverByEmailPasswordAsync(string email, string password);
}
