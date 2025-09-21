using AutoMapper;
using OrderOperations.Application.DTOs.ProductDTOs;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Profiles;

public class ProductMappingProfiles : Profile
{
    public ProductMappingProfiles()
    {
        CreateMap<CreateProductViewModel, Product>();
        CreateMap<UpdateProductViewModel, Product>();
        CreateMap<Product, ProductViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
    }
}

