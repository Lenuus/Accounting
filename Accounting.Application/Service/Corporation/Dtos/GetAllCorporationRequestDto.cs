using AccountingsTracker.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Corporation.Dtos
{
    public class GetAllCorporationRequestDto : PagedRequestDto
    {
        [AllowNull]
        public string Number { get; set; }
        [AllowNull]
        public string VKN { get; set; }
        [AllowNull]
        public string TCKN { get; set; }
        [AllowNull]
        public string City { get; set; }
        [AllowNull]
        public string State { get; set; }
        [AllowNull]
        public string Country { get; set; }
    }
}
