using Accounting.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Management.Dtos
{
    public class GetAllUserRequestDto : PagedRequestDto
    {
        [AllowNull]
        public string Search { get; set; }
        [AllowNull]
        public List<Guid> Roles { get; set; } = new List<Guid>();
    }
}
