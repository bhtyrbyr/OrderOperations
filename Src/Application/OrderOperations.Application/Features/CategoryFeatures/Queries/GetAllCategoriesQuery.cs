using AutoMapper;
using MediatR;
using OrderOperations.Application.DTOs.CategoryDTOs;
using OrderOperations.Application.Interfaces.Repositories;

namespace OrderOperations.Application.Features.CategoryFeatures.Queries;

public record GetAllCategoriesQuery() : IRequest<List<CategoryViewModel>>;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryViewModel>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<CategoryViewModel>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<CategoryViewModel>>(categories);
    }
}
