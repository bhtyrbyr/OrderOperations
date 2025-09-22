using AutoMapper;
using OrderOperations.Application.DTOs.BasketDTOs;
using OrderOperations.Application.DTOs.OrderDTOs;
using OrderPoerations.Domain.Entities;

namespace OrderOperations.Application.Profiles;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.PersonId))
            .ForMember(dest => dest.BasketId, opt => opt.MapFrom(src => src.Basket.Id))
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost))
            .ForMember(dest => dest.Fail, opt => opt.MapFrom(src => src.Fail))
            .ForMember(dest => dest.IdempotencyKey, opt => opt.MapFrom(src => src.IdempotencyKey));

        CreateMap<OrderItem, OrderItemViewModel>()
            .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost));
    }
}
