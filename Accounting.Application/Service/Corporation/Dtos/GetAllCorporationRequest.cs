using AccountingsTracker.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Corporation.Dtos
{
    public class GetAllCorporationRequest:PagedRequestDto
    {
        public string? Search { get; set; }
    }
}
