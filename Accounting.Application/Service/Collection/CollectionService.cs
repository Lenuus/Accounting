﻿using Accounting.Application.Service.Collection.Dtos;
using Accounting.Application.Service.Corporation.Dtos;
using Accounting.Common.Enum;
using Accounting.Common.Helpers;
using Accounting.Domain;
using Accounting.Common.Dtos;
using Accounting.Common.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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

        public async Task<ServiceResponse> CreateCollection(CollectionCreateRequestDto request)
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
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    mappedCollection.TenantId = _claimManager.GetTenantId();
                    var collection = await _collectionRepository.Create(mappedCollection).ConfigureAwait(false);
                    var record = await CreateCollectionRecord(mappedCollection).ConfigureAwait(false);
                    if (!record.IsSuccesfull)
                    {
                        return new ServiceResponse(false, "Record cannot be created");
                    }
                    var collectionDocuments = request.CollectionDocuments.Select(documentDto => new Domain.CollectionDocument
                    {
                        CollectionId = collection.Id,
                        Number = documentDto.Number,
                        LastDate = documentDto.LastDate,
                        Price = documentDto.Price,
                        TenantId = _claimManager.GetTenantId()
                    }).ToList();

                    foreach (var collectionDocument in collectionDocuments)
                    {
                        await _collectionDocumentRepository.Create(collectionDocument).ConfigureAwait(false);
                    }
                    scope.Complete();
                }
                catch (Exception)
                {

                    return new ServiceResponse(false, "Collection Could not Created");
                }
            }
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> DeleteCollection(Guid id)
        {
            var collection = await _collectionRepository.GetById(id).ConfigureAwait(false);
            if (collection == null)
            {
                return new ServiceResponse(false, "Requested collection cannot found");
            }
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _collectionRepository.Delete(collection).ConfigureAwait(false);

                    var colDoc = _collectionDocumentRepository.GetAll().Where(f => f.CollectionId == id).ToList();
                    if (colDoc.Any())
                    {
                        foreach (var document in colDoc)
                        {
                            await _collectionDocumentRepository.DeleteById(document.Id).ConfigureAwait(false);
                        }
                    }
                    var corpRec = _corporationRecordRepository.GetByReferenceId(id).ToList();
                    foreach (var document in corpRec)
                    {
                        await _corporationRecordRepository.DeleteById(document.Id).ConfigureAwait(false);
                    }
                    scope.Complete();
                }
                catch (Exception)
                {

                    return new ServiceResponse(false, "Delete process failed");
                }
            }

            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> DeleteCollectionDocument(Guid id)
        {
            var collectionDocument = await _collectionDocumentRepository.GetById(id).ConfigureAwait(false);
            if (collectionDocument == null)
            {
                return new ServiceResponse(false, "Requested collection cannot found");
            }
            await _collectionDocumentRepository.Delete(collectionDocument).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse<PagedResponseDto<CollectionListDto>>> GetAllCollections(GetAllCollectionRequest request)
        {

            var loggedUserId = _claimManager.GetUserId();
            var loggedTenantId = _claimManager.GetTenantId();

            var query = _collectionRepository.GetAll().Include(f => f.CollectionDocuments)
                                              .Where(f => !f.IsDeleted && f.TenantId == loggedTenantId &&
                                              (string.IsNullOrEmpty(request.Number) || f.Number.Contains(request.Number)));
            if (request.SortDirection == SortDirection.Descending)
            {
                query.OrderByDescending(f => f.Date);
            }
            if (request.SortDirection == SortDirection.Ascending)
            {
                query.OrderByDescending(f => f.Date);
                query.OrderBy(f => f.Date);
            }
            var collection = await query.Select(f => new CollectionListDto
            {
                Id = f.Id,
                CorporationId = f.Id,
                InOut = f.InOut,
                Date = f.Date,
                LastDate = f.LastDate,
                Number = f.Number,
                PaymentType = f.PaymentType,
                TotalPrice = f.TotalPrice,
                CollectionDocuments = f.CollectionDocuments.Where(x => !x.IsDeleted).Select(x => new CollectionDocumentListDto
                {
                    Id = x.Id,
                    CollectionId = x.CollectionId,
                    LastDate = x.LastDate,
                    Number = x.Number,
                    Price = x.Price,

                }).ToList(),
            }).ToPagedListAsync(request.PageSize, request.PageIndex).ConfigureAwait(false);

            return new ServiceResponse<PagedResponseDto<CollectionListDto>>(collection, true, string.Empty);
        }

        public async Task<ServiceResponse> UpdateCollection(CollectionUpdateRequestDto request)
        {
            var collection = await _collectionRepository.GetById(request.Id).ConfigureAwait(false);
            if (collection == null)
            {
                return new ServiceResponse(false, "Collection cannot be found");
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    collection.Number = request.Number;
                    collection.CorporationId = request.CorporationId;
                    collection.InOut = request.InOut;
                    collection.Date = request.Date;
                    collection.LastDate = request.LastDate;
                    collection.PaymentType = request.PaymentType;
                    collection.TotalPrice = request.TotalPrice;

                    await _collectionRepository.Update(collection).ConfigureAwait(false);
                    if (request.Documents.Any())
                    {


                        foreach (var documentDto in request.Documents)
                        {
                            var collectionDocument = await _collectionDocumentRepository.GetById(documentDto.Id).ConfigureAwait(false);
                            if (collectionDocument == null || collectionDocument.CollectionId != collection.Id)
                            {
                                return new ServiceResponse(false, $"CollectionDocument with Number {documentDto.Number} cannot be found");
                            }

                            if (collectionDocument.LastDate != documentDto.LastDate ||
                                collectionDocument.Number != documentDto.Number ||
                                collectionDocument.Price != documentDto.Price)
                            {
                                collectionDocument.CollectionId = collection.Id;
                                collectionDocument.LastDate = documentDto.LastDate;
                                collectionDocument.Number = documentDto.Number;
                                collectionDocument.Price = documentDto.Price;

                                await _collectionDocumentRepository.Update(collectionDocument).ConfigureAwait(false);
                                var corpRecord = await updateCorpRecord(collection).ConfigureAwait(false);
                                if (!corpRecord.IsSuccesfull)
                                {
                                    return new ServiceResponse(false, "Corp record update failed");
                                }
                            }
                        }
                    }
                    scope.Complete();
                    return new ServiceResponse(true, "Collection updated successfully");
                }
                catch (Exception ex)
                {

                    return new ServiceResponse(false, $"An error occurred: {ex.Message}");
                }
            }
        }

        private async Task<ServiceResponse> updateCorpRecord(Domain.Collection collection)
        {
            var corpRecord = await _corporationRecordRepository.GetCorpRecordByReferenceId(collection.Id).ConfigureAwait(false);
            if (corpRecord == null)
            {
                return new ServiceResponse(false, "Corporation Record cannot found");
            }
            corpRecord.ReferenceId = collection.Id;
            corpRecord.ActDate = collection.Date;
            corpRecord.LastDate = collection.LastDate;
            corpRecord.CorporationId = collection.CorporationId;
            corpRecord.TenantId = _claimManager.GetTenantId();
            corpRecord.Price = collection.TotalPrice;
            corpRecord.InOut = collection.InOut;
            switch (collection.PaymentType)
            {
                case PaymentType.PromptNote:
                    corpRecord.ActType = ActType.PromptNote;
                    break;
                case PaymentType.Check:
                    corpRecord.ActType = ActType.Check;
                    break;
                case PaymentType.Cash:
                    corpRecord.ActType = ActType.Cash;
                    break;
                case PaymentType.CreditCard:
                    corpRecord.ActType = ActType.CreditCard;
                    break;
                case PaymentType.Transfer:
                    corpRecord.ActType = ActType.Transfer;
                    break;
                default:
                    corpRecord.ActType = ActType.None;
                    break;
            }
            await _corporationRecordRepository.Update(corpRecord).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        private async Task<ServiceResponse> CreateCollectionRecord(Domain.Collection mappedCollection)
        {
            var collectionRecord = new CorporationRecordCreateRequestDto
            {
                CorporationId = mappedCollection.CorporationId,
                ActDate = mappedCollection.Date,
                LastDate = mappedCollection.LastDate,
                InOut = mappedCollection.InOut,
                Price = mappedCollection.TotalPrice,
                ReferenceId = mappedCollection.Id,
                TenantId = _claimManager.GetTenantId(),

            };

            switch (mappedCollection.PaymentType)
            {
                case PaymentType.PromptNote:
                    collectionRecord.ActType = ActType.PromptNote;
                    break;
                case PaymentType.Check:
                    collectionRecord.ActType = ActType.Check;
                    break;
                case PaymentType.Cash:
                    collectionRecord.ActType = ActType.Cash;
                    break;
                case PaymentType.CreditCard:
                    collectionRecord.ActType = ActType.CreditCard;
                    break;
                case PaymentType.Transfer:
                    collectionRecord.ActType = ActType.Transfer;
                    break;
                default:
                    collectionRecord.ActType = ActType.None; // Varsayılan değer
                    break;
            }


            var record = _mapper.Map<Domain.CorporationRecord>(collectionRecord);
            if (record == null)
            {
                return new ServiceResponse(false, "Record cannot created");
            }
            await _corporationRecordRepository.Create(record).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }
    }
}
