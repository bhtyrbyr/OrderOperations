using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Interfaces.Repositories;

public interface IIdentityService
{
    Task<Guid> RegisterUserAsync(Person person, string email, string password);
}
