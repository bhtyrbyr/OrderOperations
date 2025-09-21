using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OrderOperations.Application.DTOs.AuthorizationDTOs;
using OrderOperations.CustomExceptions.Exceptions.AuthExceptions;
using OrderPoerations.Domain.Entities;
using System;

namespace OrderOperations.Application.Features.Authorization.Commands;

public record RegisterUserCommand(RegisterUserViewModel Model) : IRequest<string>;
public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly UserManager<Person> _userManager;
    private readonly IMapper _mapper;

    public RegisterUserHandler(UserManager<Person> userManager, IMapper mapper)
    {
        _userManager = userManager;
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
        await _userManager.AddToRoleAsync(newUser, "Customer");
        return newUser.Id.ToString();
    }
}