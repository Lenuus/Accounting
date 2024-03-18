using Accounting.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Corporation.Dtos
{
    public class CorporationRecordCreateRequestDto
    {
        public Guid CorporationId { get; set; }
        public DateTime ActDate { get; set; }
        public DateTime LastDate { get; set; }
        public ActType ActType { get; set; }
        public bool InOut { get; set; }
        public decimal Price { get; set; }
        public Guid ReferenceId { get; set; }
        public Guid TenantId { get; set; } = Guid.Empty;
    }
}
