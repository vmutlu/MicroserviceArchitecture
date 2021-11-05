using AutoMapper;
using DotnetMicroserviceArchitecture.Order.Application.DTOs;

namespace DotnetMicroserviceArchitecture.Order.Application.Mapping
{
    public class Map : Profile
    {
        public Map()
        {
            CreateMap<DotnetMicroserviceArchitecture.Order.Domain.Aggregate.Order, OrderDTO>().ReverseMap();    
            CreateMap<DotnetMicroserviceArchitecture.Order.Domain.Aggregate.OrderItem, OrderItemDTO>().ReverseMap();    
            CreateMap<DotnetMicroserviceArchitecture.Order.Domain.Aggregate.Adress, AdressDTO>().ReverseMap();    
        }
    }
}
