using Accounting.Application.Service.Collection.Dtos;
using Accounting.Application.Service.Corporation.Dtos;
using AccountingsTracker.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Collection
{
    public interface ICollectionService : IApplicationService
    {
        Task<ServiceResponse> CreateCollection(CreateCollectionRequestDto request);
        Task<ServiceResponse> DeleteCollection(Guid id);
        Task<ServiceResponse> UpdateCollection(UpdateCollectionRequestDto request);
        Task<ServiceResponse<PagedResponseDto<CollectionListDto>>> GetAllCollections(GetAllCollectionRequest request);
        Task<ServiceResponse> DeleteCollectionDocument(Guid id);
    }
}
