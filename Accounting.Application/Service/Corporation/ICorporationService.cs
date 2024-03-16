using Accounting.Application.Service.Corporation.Dtos;
using Accountings.Common.Dtos;
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
        Task<ServiceResponse> CreateCorporation(CorporationRegisterRequestDto request);
        Task<ServiceResponse> RemoveCorporation(Guid id);
        Task<ServiceResponse<PagedResponseDto<CorporationListDto>>> GetAllCorporations(GetAllCorporationRequest request);
        Task<ServiceResponse> UpdateCorporation(UpdateCorporationDto request);


    }
}
