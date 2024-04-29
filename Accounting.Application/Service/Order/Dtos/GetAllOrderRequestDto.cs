using Accounting.Common.Enum;
using Accounting.Common.Dtos;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Order.Dtos
{
    public class GetAllOrderRequestDto : PagedRequestDto
    {
        [AllowNull]
        public string Number { get; set; }
        [AllowNull]
        public DateTime? StartDate { get; set; }
        [AllowNull]
        public DateTime? EndDate { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
