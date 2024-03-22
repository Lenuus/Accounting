using Accounting.Application.Service.Collection.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Collection.Mapping
{
    public class CollectionMapper : Profile
    {
        public CollectionMapper()
        {
            CreateMap<CreateCollectionRequestDto, Domain.Collection>().ForMember(dest => dest.CollectionDocuments, opt => opt.Ignore());
            CreateMap<CreateCollectionDocumentRequestDto, Domain.CollectionDocument>();
        }
    }
}
