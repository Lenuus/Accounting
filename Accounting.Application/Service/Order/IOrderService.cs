﻿using Accounting.Application.Service.Corporation.Dtos;
using Accounting.Application.Service.Order.Dtos;
using Accounting.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Order
{
    public interface IOrderService : IApplicationService
    {
        Task<ServiceResponse> CreateOrder(OrderCreateRequestDto request);
        Task<ServiceResponse> RemoveOrder(Guid id);
        Task<ServiceResponse> CancelOrder(Guid id);
        Task<ServiceResponse> UpdateOrder(OrderUpdateRequestDto request);
        Task<ServiceResponse<PagedResponseDto<OrderListDto>>> GetAllOrder(GetAllOrderRequestDto request);


    }
}
