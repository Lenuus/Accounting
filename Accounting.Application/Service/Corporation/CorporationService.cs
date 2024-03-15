using Accounting.Application.Service.ClaimManager;
using Accounting.Application.Service.Corporation.Dtos;
using Accounting.Application.Service.Order;
using AccountingsTracker.Common.Constants;
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
        private readonly IRepository<Domain.Tenant> _tenantRepository;
        private readonly IMapper _mapper;
        private readonly IClaimManager _claimManager;

        public CorporationService(IRepository<Domain.Corporation> corporationRepository, IMapper mapper, IClaimManager claimManager, IRepository<Domain.Order> orderRepository, IRepository<Domain.CorporationRecord> corporationrecordRepository, IRepository<Domain.Tenant> tenantRepository)
        {
            _corporationRepository = corporationRepository;
            _mapper = mapper;
            _claimManager = claimManager;
            _orderRepository = orderRepository;
            _corporationrecordRepository = corporationrecordRepository;
            _tenantRepository = tenantRepository;
        }

        public async Task<ServiceResponse<PagedResponseDto<CorporationListDto>>> GetAllCorporations(GetAllCorporationRequest request)
        {
            var loggedUserId = _claimManager.GetUserId();
            var query = await _corporationRepository.GetAll()
                 .Include(f => f.Order).Include(f => f.CorporationRecord)
                 .Where(f => !f.IsDeleted && (!string.IsNullOrEmpty(request.Search) ?
                 EF.Functions.Like(EF.Functions.Collate(f.Number, "SQL_Latin1_General_CP1_CI_AS"), $"%{request.Search}%") : true)
                 && request.TenantId != null ? f.TenantId == request.TenantId : true)
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
                     CorporationRecord = f.CorporationRecord.Where(f => !f.IsDeleted).Select(f => new CorporationRecordDetailDto
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
                     Order = f.Order.Where(f => !f.IsDeleted).Select(f => new OrderDetailDto
                     {
                         Id = f.Id,
                         CorporationId = f.CorporationId,
                         Date = f.Date,
                         LastDate = f.LastDate,
                         NetPrice = f.NetPrice,
                         Number = f.Number,
                         TenantId = f.TenantId,
                         TotalDiscount = f.TotalDiscount,
                         TotalPrice = f.TotalPrice,
                     }).ToList(),

                 }).ToPagedListAsync(request.PageSize, request.PageIndex).ConfigureAwait(false);

            return new ServiceResponse<PagedResponseDto<CorporationListDto>>(query, true, string.Empty);
        }

        public async Task<ServiceResponse> RegisterCorporation(CorporationRegisterRequestDto request)
        {
            if (request == null)
            {
                return new ServiceResponse(false, "Request is not valid");
            }
            if (request.Title && request.VKN.IsNullOrEmpty())
            {
                return new ServiceResponse(false, "For corporations VKN number is required.");
            }
            if (request.Title == false && !(request.VKN.IsNullOrEmpty()))
            {
                return new ServiceResponse(false, "For personel corporations VKN is not needed.");

            }
            if (request.Title && !(request.TCKN.IsNullOrEmpty()))
            {
                return new ServiceResponse(false, "For  corporations TCKN number is not needed.");

            }
            if (request.Title == false && request.TCKN.IsNullOrEmpty())
            {
                return new ServiceResponse(false, "For personel corporations TCKN number is required.");
            }

            var entity = _mapper.Map<Domain.Corporation>(request);
            entity.TenantId = Guid.NewGuid();
            var userRole = _claimManager.GetRole();
            if (userRole == RoleConstants.Admin)
            {
                entity.InsertedById = _claimManager.GetUserId();
                entity.TenantId = _claimManager.GetTenantId();
                await _corporationRepository.Create(entity).ConfigureAwait(false);
            }
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> RemoveCorporation(Guid id)
        {
            try
            {
                var userRole = _claimManager.GetRole();
                if (userRole == RoleConstants.Admin)
                {
                    await _corporationRepository.DeleteById(id).ConfigureAwait(false);
                    var entity = await _corporationRepository.GetById(id).ConfigureAwait(false);
                    entity.DeletedById = _claimManager.GetUserId();
                }
                return new ServiceResponse(true, string.Empty);
            }
            catch (Exception)
            {
                return new ServiceResponse(false, "The Corporation You are trying to delete is not registered or cant be found");
            }
        }

    }
}
