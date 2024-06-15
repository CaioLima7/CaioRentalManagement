using AutoMapper;
using EventBusRabbitMQ.Events;
using Ordering.Application.Commands;
using Ordering.Application.Responses;
using Ordering.Core.Entities;

namespace Ordering.API.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<RentalCheckoutEvent, CreateOrderCommand>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.RentalStartDate))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.RentalItems));

            CreateMap<CreateOrderCommand, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<RentalCheckoutEvent, CreateOrderCommand>()
                  .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.RentalStartDate))
                  .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.RentalItems));

            CreateMap<RentalItem, OrderItems>();
        }
    }
}
