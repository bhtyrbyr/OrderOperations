using AutoMapper;
using MediatR;
using OrderOperations.Application.DTOs.CategoryDTOs;
using OrderOperations.Application.Interfaces.Repositories;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Features.CategoryFeatures.Commands;

public record CreateCategoryCommand(CreateCategoryViewModel Model) : IRequest<int>;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(request.Model);
        await _categoryRepository.CreateAsync(category, cancellationToken);
        return category.Id;
    }
}
