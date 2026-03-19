using Kmm.OrderService.Application.Common.Models;
using Kmm.OrderService.Application.Orders.CancelOrder.Dtos;
using Kmm.OrderService.Application.Orders.ConfirmOrder.Dtos;
using Kmm.OrderService.Application.Orders.CreateOrder.Dtos;
using Kmm.OrderService.Application.Orders.GetOrder.Dtos;
using Kmm.OrderService.Application.Orders.ListOrder.Dtos;
using Kmm.OrderService.Application.Products.Shared.Dtos;
using Kmm.OrderService.Application.Shared.Dtos;
using Kmm.OrderService.Domain.Entities;
using Kmm.OrderService.Domain.Enums;

namespace Kmm.OrderService.Application.Common.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Enum
        CreateMap<EOrderStatus, string>().ConvertUsing(s => s.ToString());

        // Products
        CreateMap<Product, ProductDto>();

        // Orders
        CreateMap<Order, CreateOrderDto>();

        CreateMap<Order, ConfirmOrderDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status));

        CreateMap<Order, CancelOrderDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status));

        CreateMap<OrderItem, OrderItemDto>();

        CreateMap<Order, OrderDto>()
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.OrderItems))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status));

        CreateMap<Order, GetOrderDto>()
            .IncludeBase<Order, OrderDto>();

        // Generic 
        CreateMap(typeof(PaginatedList<>), typeof(PaginatedListDto<>));
    }
}
