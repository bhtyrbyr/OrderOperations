using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OrderOperations.Application.DTOs.AuthorizationDTOs;
using OrderOperations.CustomExceptions.Exceptions.AuthExceptions;
using OrderPoerations.Domain.Entities;
using System;

namespace OrderOperations.Application.Features.AuthorizationFeatures.Commands;

public record RegisterUserCommand(RegisterUserViewModel Model) : IRequest<string>;
public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly UserManager<Person> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IMapper _mapper;

    public RegisterUserHandler(UserManager<Person> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var oldUser = await _userManager.FindByEmailAsync(request.Model.Email);
        if (oldUser is not null)
        {
            throw new UserAlreadyExistException("userMailAlreadyExistErrMsg", param1: "modulNameMsg*AuthorizationModule", param2: "matchedDataMsg*Email");
        }
        oldUser = await _userManager.FindByNameAsync(request.Model.UserName);
        if (oldUser is not null)
        {
            throw new UserAlreadyExistException("userNameAlreadyExixstErrMsg", param1: "modulNameMsg*AuthorizationModule", param2: "matchedDataMsg*Username");
        }
        var newUser = _mapper.Map<Person>(request.Model);
        await _userManager.CreateAsync(newUser, request.Model.Password);

        var roles = _roleManager.Roles.Select(role => role.Name).ToList();

        if (!roles.Contains("Admin"))
        {
            var adminRole = new IdentityRole("Admin");
            adminRole.ConcurrencyStamp = "admin";
            await _roleManager.CreateAsync(adminRole);
        }
        if(!roles.Contains("Customer"))
        {
            var customerRole = new IdentityRole("Customer");
            customerRole.ConcurrencyStamp = "customer";
            await _roleManager.CreateAsync(customerRole);
        }

        await _userManager.AddToRoleAsync(newUser, "Customer");
        if(newUser.UserName.Equals("admin"))
            await _userManager.AddToRoleAsync(newUser, "Admin");
        return newUser.Id.ToString();
    }
}