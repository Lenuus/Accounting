using Accounting.Application.Service.Order.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Order.Mapping
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            CreateMap<OrderCreateRequestDto, Domain.Order>().ForMember(dest => dest.Products, opt => opt.Ignore());
        }
    }
}
