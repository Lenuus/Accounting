using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Management.Dtos
{
    public class UserUpdateRequestDto
    {
        public Guid Id { get; set; }
        [AllowNull]
        public string Email { get; set; }
        [AllowNull]
        public string PhoneNumber { get; set; }
        [AllowNull]
        public List<Guid> Roles { get; set; } = new List<Guid>();
    }
}
