using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Repositories;

public interface IIdentityService
{
    Task<Guid> RegisterUserAsync(Person person, string email, string password);
}
