using MediatR;
using OrderOperations.Application.Repositories;
using OrderOperations.CustomExceptions.Exceptions.CommonExceptions;

namespace OrderOperations.Application.Features.CategoryFeatures.Commands;

public record DeleteCategoryCommand(int Id) : IRequest<bool>;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null)
        {
            throw new NotFoundException("categoryNotFoundMsg", param1: "modulNameMsg*CategoryModule");
        }

        _categoryRepository.Delete(category);
        return true;
    }
}
