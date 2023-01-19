using AutoMapper;
using DutchTreat.Data.Entities;
using System.Runtime.InteropServices;

namespace DutchTreat.Dtos
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(o => o.OrderId, e => e.MapFrom( e => e.Id))
                .ReverseMap();
            CreateMap<OrderItem, OrderItemDto>()
                .ReverseMap();
        }
    }
}
