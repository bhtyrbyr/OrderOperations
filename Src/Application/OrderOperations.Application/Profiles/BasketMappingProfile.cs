using AutoMapper;
using OrderOperations.Application.DTOs.BasketDTOs;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Profiles;

public class BasketMappingProfile : Profile
{
    public BasketMappingProfile()
    {
        CreateMap<Basket, BasketViewModel>()
            .ForMember(dest => dest.BasketItems, opt => opt.MapFrom(src => src.BasketItems));

        CreateMap<BasketItem, BasketItemViewModel>()
            .ForMember(dest => dest.BasketItemId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost));
    }
}
