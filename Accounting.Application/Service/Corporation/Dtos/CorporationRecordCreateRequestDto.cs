using Accounting.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Corporation.Dtos
{
    public class CorporationRecordCreateRequestDto
    {
        [Required]
        public Guid CorporationId { get; set; }
        [Required]
        public DateTime ActDate { get; set; }
        [Required]
        public DateTime LastDate { get; set; }
        [Required]
        public ActType ActType { get; set; }
        [Required]
        public bool InOut { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public Guid ReferenceId { get; set; }
        [AllowNull]
        public Guid TenantId { get; set; }

    }
}
