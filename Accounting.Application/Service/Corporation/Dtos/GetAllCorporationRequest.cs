using AccountingsTracker.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Corporation.Dtos
{
    public class GetAllCorporationRequest : PagedRequestDto
    {
        public string Number { get; set; }
        public string VKN { get; set; }
        public string TCKN { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
