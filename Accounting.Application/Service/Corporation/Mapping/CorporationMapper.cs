using Accounting.Application.Service.Corporation.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Corporation.Mapping
{
    public class CorporationMapper:Profile
    {       
        public CorporationMapper() 
        {
            CreateMap<CorporationRegisterRequestDto, Domain.Corporation>();
            CreateMap<CorporationRecordCreateRequestDto,Domain.CorporationRecord>();
        }

    }
}
