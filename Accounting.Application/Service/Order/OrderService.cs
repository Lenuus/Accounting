using Accounting.Application.Service.Corporation.Dtos;
using Accounting.Application.Service.Order.Dtos;
using Accounting.Common.Helpers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Order
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Domain.Order> _orderRepository;
        private readonly IMapper _mapper;
        private readonly IClaimManager _claimManager;
        private readonly IRepository<Domain.CorporationRecord> _corporationRecordRepository;

        public OrderService(IRepository<Domain.Order> orderRepository, IMapper mapper, IClaimManager claimManager, IRepository<Domain.CorporationRecord> corporationRecordRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _claimManager = claimManager;
            _corporationRecordRepository = corporationRecordRepository;
        }

        public async Task<ServiceResponse> CreateOrder(OrderCreateRequestDto request)
        {
            if (request == null)
            {
                return new ServiceResponse(false, "Request is not valid");
            }
            var mappedRequest = _mapper.Map<Domain.Order>(request);
            mappedRequest.TenantId = _claimManager.GetTenantId();
            var order = await _orderRepository.Create(mappedRequest).ConfigureAwait(false);
            var record = await OrderRecord(order).ConfigureAwait(false);
            if (!record.IsSuccesfull)
            {
                return new ServiceResponse(false, "Record cannot be created");
            }
            return new ServiceResponse(true, string.Empty);
        }


        public async Task<ServiceResponse> RemoveOrder(Guid id)
        {
            var order = await _orderRepository.GetById(id).ConfigureAwait(false);
            if (order == null)
            {
                return new ServiceResponse(false, "Cant found");
            }
            await _corporationRecordRepository.DeleteById(order.Id).ConfigureAwait(false);
            await _orderRepository.DeleteById(id).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> UpdateOrder(UpdateOrderRequestDto request)
        {
            if (request == null)
            {
                return new ServiceResponse(false, "request is not valid");
            }

            var order = await _orderRepository.GetById(request.Id).ConfigureAwait(false);
            if (order == null)
            {
                return new ServiceResponse(false, "requested order cannot be found");
            }
            order.Id = request.Id;
            order.NetPrice = request.NetPrice;
            order.TotalPrice = request.TotalPrice;
            order.TotalDiscount = request.TotalDiscount;
            order.CorporationId = request.CorporationId;
            order.ActType = request.ActType;
            order.LastDate = request.LastDate;
            order.Date = request.Date;
            order.Number = request.Number;

            var orderCorpRecord = await _corporationRecordRepository.GetById(order.Id).ConfigureAwait(false);
            if (orderCorpRecord == null)
            {
                return new ServiceResponse(false, "Order record cannot found");
            }
            orderCorpRecord.ReferenceId = order.Id;
            orderCorpRecord.CorporationId = order.CorporationId;
            orderCorpRecord.ActDate = order.Date;
            orderCorpRecord.LastDate = order.Date;
            orderCorpRecord.Price = order.NetPrice;
            orderCorpRecord.ActType = order.ActType;
            await _orderRepository.Update(order).ConfigureAwait(false);
            await _corporationRecordRepository.Update(orderCorpRecord).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        private async Task<ServiceResponse> OrderRecord(Domain.Order order)
        {
            var corpOrderRecord = new CorporationRecordCreateRequestDto
            {
                ActDate = order.Date,
                CorporationId = order.CorporationId,
                LastDate = order.LastDate,
                Price = order.NetPrice,
                ActType = order.ActType,
                InOut = true,
                TenantId = order.TenantId,
                ReferenceId = order.Id
            };
            var record = _mapper.Map<Domain.CorporationRecord>(corpOrderRecord);
            if (record == null)
            {
                return new ServiceResponse(false, "Record cannot created");
            }
            await _corporationRecordRepository.Create(record).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }
    }
}
