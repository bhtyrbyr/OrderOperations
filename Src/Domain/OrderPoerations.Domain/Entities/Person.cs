using Microsoft.AspNetCore.Identity;

namespace OrderPoerations.Domain.Entities;

public class Person : IdentityUser
{
    public string FullName { get; set; }
}
