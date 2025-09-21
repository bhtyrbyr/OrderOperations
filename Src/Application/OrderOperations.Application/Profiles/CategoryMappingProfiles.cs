using AutoMapper;
using OrderOperations.Application.DTOs.CategoryDTOs;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Profiles;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<CreateCategoryViewModel, Category>();
        CreateMap<Category, CategoryViewModel>();
    }
}
