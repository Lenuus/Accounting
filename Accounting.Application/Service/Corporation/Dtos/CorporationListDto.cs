using Accounting.Application.Service.Order.Dtos;
using Accounting.Common.Enum;
using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Corporation.Dtos
{
    public class CorporationListDto
    {
        public string Number { get; set; }
        public bool Title { get; set; }
        public string TCKN { get; set; }
        public string VKN { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public decimal CurrentBalance { get; set; }
        public CorporationType CorporationType { get; set; }
        public ICollection<CorporationRecordDetailDto> CorporationRecord { get; set; }
        public ICollection<OrderDetailDto> Order { get; set; }
        public Guid TenantId { get; set; }
    }
}
