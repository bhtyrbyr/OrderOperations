using System.Security.Claims;

namespace OrderOperations.Application.TokenOperations;

public interface IAuthService
{
    Task<(string Id, (string token, DateTime StartDate, DateTime EndDate))> Login(string userName, string password);
    (string token, DateTime StartDate, DateTime EndDate) CreateAccessToken(IEnumerable<Claim> claims);
}