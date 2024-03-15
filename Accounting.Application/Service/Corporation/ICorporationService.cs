using Accounting.Application.Service.Corporation.Dtos;
using AccountingsTracker.Common.Dtos;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Corporation
{
    public interface ICorporationService : IApplicationService
    {
        Task<ServiceResponse> RegisterCorporation(CorporationRegisterRequestDto request);
        Task<ServiceResponse> RemoveCorporation(Guid id);
        Task<ServiceResponse<PagedResponseDto<CorporationListDto>>> GetAllCorporations(GetAllCorporationRequest request);

    }
}
