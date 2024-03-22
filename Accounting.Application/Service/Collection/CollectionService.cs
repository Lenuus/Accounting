using Accounting.Application.Service.Collection.Dtos;
using Accounting.Application.Service.Corporation.Dtos;
using Accounting.Common.Enum;
using Accounting.Common.Helpers;
using Accounting.Domain;
using AccountingsTracker.Common.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Collection
{
    public class CollectionService : ICollectionService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Domain.Collection> _collectionRepository;
        private readonly IRepository<Domain.CollectionDocument> _collectionDocumentRepository;
        private readonly IRepository<Domain.CorporationRecord> _corporationRecordRepository;
        private readonly IClaimManager _claimManager;

        public CollectionService(IRepository<CorporationRecord> corporationRecordRepository, IRepository<CollectionDocument> collectionDocumentRepository, IRepository<Domain.Collection> collectionRepository, IMapper mapper, IClaimManager claimManager)
        {
            _corporationRecordRepository = corporationRecordRepository;
            _collectionDocumentRepository = collectionDocumentRepository;
            _collectionRepository = collectionRepository;
            _mapper = mapper;
            _claimManager = claimManager;
        }

        public async Task<ServiceResponse> CreateCollection(CreateCollectionRequestDto request)
        {
            if (request == null)
            {
                return new ServiceResponse(false, "Request is not valid");
            }
            var mappedCollection = _mapper.Map<Domain.Collection>(request);
            if (mappedCollection == null)
            {
                return new ServiceResponse(false, "Collection is not valid");
            }
            mappedCollection.TenantId = _claimManager.GetTenantId();
            var collection = await _collectionRepository.Create(mappedCollection).ConfigureAwait(false);
            var record = await CreateCollectionRecord(mappedCollection).ConfigureAwait(false);
            if (!record.IsSuccesfull)
            {
                return new ServiceResponse(false, "Record cannot be created");
            }

            return new ServiceResponse(true, string.Empty);
        }

        private async Task<ServiceResponse> CreateCollectionRecord(Domain.Collection mappedCollection)
        {
            var collectionRecord = new CorporationRecordCreateRequestDto
            {
                CorporationId = mappedCollection.CorporationId,
                ActDate = mappedCollection.Date,
                LastDate = mappedCollection.LastDate,
                InOut = true,
                Price = mappedCollection.TotalPrice,
                ReferenceId = mappedCollection.Id,
                TenantId = _claimManager.GetTenantId(),
                ActType = ActType.Transfer //TODO:Sorulacak acttype ve paymenttype olayı
            };
            var record = _mapper.Map<Domain.CorporationRecord>(collectionRecord);
            if (record == null)
            {
                return new ServiceResponse(false, "Record cannot created");
            }
            await _corporationRecordRepository.Create(record).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public Task<ServiceResponse> CreateCollectionDocument(CreateCollectionDocumentRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> DeleteCollection(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> DeleteCollectionDocument(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<PagedResponseDto<CollectionListDto>>> GetAllCollections(GetAllCollectionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> UpdateCollection(UpdateCollectionRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> UpdateCollectionDocument(UpdateCollectionDocumentRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
