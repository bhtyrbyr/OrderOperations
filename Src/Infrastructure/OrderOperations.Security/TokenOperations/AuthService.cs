using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderOperations.Application.Interfaces.TokenOperations;
using OrderPoerations.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderOperations.Security.TokenOperations;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<Person> _userManager;
    private readonly SignInManager<Person> _signingManager;

    public AuthService(IConfiguration configuration, UserManager<Person> userManager, SignInManager<Person> signingManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signingManager = signingManager;
    }

    public async Task<(string Id, (string token, DateTime StartDate, DateTime EndDate))> Login(string userName, string Password)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return (string.Empty, ("errorLogin", DateTime.UtcNow, DateTime.UtcNow));

        var result = await _signingManager.PasswordSignInAsync(user, Password, false, false);

        if (!result.Succeeded)
            return (string.Empty, ("errorLogin", DateTime.UtcNow, DateTime.UtcNow));


        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }


        var token = CreateAccessToken(claims);

        return (user.Id, token);
    }

    public (string token, DateTime StartDate, DateTime EndDate) CreateAccessToken(IEnumerable<Claim> claims)
    {
        var TokenHandler = new JwtSecurityTokenHandler();
#pragma warning disable CS8604 // Olası null başvuru bağımsız değişkeni.
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
#pragma warning restore CS8604 // Olası null başvuru bağımsız değişkeni.
        var tokenDesc = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            NotBefore = DateTime.UtcNow
        };
        var token = TokenHandler.CreateToken(tokenDesc);
        return (TokenHandler.WriteToken(token), DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
    }
}
