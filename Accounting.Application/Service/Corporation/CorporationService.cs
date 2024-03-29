using Accounting.Application.Service.Corporation.Dtos;
using Accounting.Application.Service.Order;
using Accounting.Application.Service.Order.Dtos;
using Accounting.Common.Helpers;
using Accounting.Domain;
using AccountingsTracker.Common.Dtos;
using AccountingsTracker.Common.Helpers;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Corporation
{
    public class CorporationService : ICorporationService
    {
        private readonly IRepository<Domain.Corporation> _corporationRepository;
        private readonly IRepository<Domain.Order> _orderRepository;
        private readonly IRepository<Domain.CorporationRecord> _corporationrecordRepository;
        private readonly IMapper _mapper;
        private readonly IClaimManager _claimManager;

        public CorporationService(
            IRepository<Domain.Corporation> corporationRepository,
            IMapper mapper,
            IClaimManager claimManager,
            IRepository<Domain.Order> orderRepository,
            IRepository<Domain.CorporationRecord> corporationrecordRepository)
        {
            _corporationRepository = corporationRepository;
            _mapper = mapper;
            _claimManager = claimManager;
            _orderRepository = orderRepository;
            _corporationrecordRepository = corporationrecordRepository;
        }

        public async Task<ServiceResponse<PagedResponseDto<CorporationListDto>>> GetAllCorporations(GetAllCorporationRequest request)
        {
            var loggedUserId = _claimManager.GetUserId();
            var loggedTenantId = _claimManager.GetTenantId();
            var query = await _corporationRepository.GetAll()
            .Include(f => f.Orders)
            .Include(f => f.CorporationRecords)
            .Where(f => !f.IsDeleted &&
                     f.TenantId == loggedTenantId &&
                     (string.IsNullOrEmpty(request.Number) || EF.Functions.Like(EF.Functions.Collate(f.Number, "SQL_Latin1_General_CP1_CI_AS"), $"%{request.Number}%")) &&
                     (string.IsNullOrEmpty(request.VKN) || f.VKN.Contains(request.VKN)) &&
                     (string.IsNullOrEmpty(request.TCKN) || f.TCKN.Contains(request.TCKN) &&
                     (string.IsNullOrEmpty(request.City) || f.City == request.City) &&
                     (string.IsNullOrEmpty(request.State) || f.State == request.State) &&
                     (string.IsNullOrEmpty(request.Country) || f.Country == request.Country)))
                   .Select(f => new CorporationListDto
                   {
                       Number = f.Number,
                       State = f.State,
                       Address = f.Address,
                       City = f.City,
                       CorporationType = f.CorporationType,
                       CurrentBalance = f.CurrentBalance,
                       TCKN = f.TCKN,
                       VKN = f.VKN,
                       Title = f.Title,
                       Country = f.Country,
                       CorporationRecord = f.CorporationRecords.Where(f => !f.IsDeleted).Select(f => new CorporationRecordDetailDto
                       {
                           Id = f.Id,
                           CorporationId = f.CorporationId,
                           ActDate = f.ActDate,
                           ActType = f.ActType,
                           LastDate = f.LastDate,
                           Price = f.Price,
                           InOut = f.InOut,
                           ReferenceId = f.ReferenceId,
                           TenantId = f.TenantId,

                       }).ToList(),

                   }).ToPagedListAsync(request.PageSize, request.PageIndex).ConfigureAwait(false);

            return new ServiceResponse<PagedResponseDto<CorporationListDto>>(query, true, string.Empty);
        }

        public async Task<ServiceResponse> CreateCorporation(CorporationRegisterRequestDto request)
        {
            if (request == null)
            {
                return new ServiceResponse(false, "Request is not valid");
            }

            if (string.IsNullOrEmpty(request.TCKN) && string.IsNullOrEmpty(request.VKN))
            {
                return new ServiceResponse(false, "Can not be null");
            }

            var entity = _mapper.Map<Domain.Corporation>(request);
            entity.TenantId = _claimManager.GetTenantId();
            await _corporationRepository.Create(entity).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> RemoveCorporation(Guid id)
        {
            try
            {
                await _corporationRepository.DeleteById(id).ConfigureAwait(false);
                return new ServiceResponse(true, string.Empty);
            }
            catch (Exception)
            {
                return new ServiceResponse(false, "The Corporation You are trying to delete is not registered or cant be found");
            }
        }

        public async Task<ServiceResponse> UpdateCorporation(UpdateCorporationDto request)
        {
            var corporation = await _corporationRepository.GetById(request.Id).ConfigureAwait(false);
            if (corporation == null)
            {
                return new ServiceResponse(false, "Not Found");
            }

            corporation.VKN = request.VKN;
            corporation.TCKN = request.TCKN;
            corporation.Number = request.Number;
            corporation.Address = request.Address;
            corporation.City = request.City;
            corporation.CorporationType = request.CorporationType;
            corporation.Country = request.Country;
            corporation.CurrentBalance = request.CurrentBalance;
            corporation.State = request.State;
            corporation.Title = request.Title;
            await _corporationRepository.Update(corporation).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> CreateCorporationRecord(CorporationRecordCreateRequestDto request)
        {
            if (request == null)
            {
                return new ServiceResponse(false, "Request is not valid");
            }
            var corp= await _corporationRepository.GetById(request.CorporationId).ConfigureAwait(false);
            if (corp==null)
            {
                return new ServiceResponse(false,"The Corporation you want is not exist in records");
            }
            var entity = _mapper.Map<Domain.CorporationRecord>(request);
            entity.TenantId = _claimManager.GetTenantId();
            await _corporationrecordRepository.Create(entity).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> RemoveCorporationRecord(Guid id)
        {
            var record = await _corporationrecordRepository.GetById(id).ConfigureAwait(false);
            if (record == null)
            {
                new ServiceResponse(false, "Can'not found");
            }
            await _corporationrecordRepository.DeleteById(id).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }
    }
}
