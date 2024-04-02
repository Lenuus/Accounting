using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Management.Dtos
{
    public class UserListDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }

        public List<RoleInfoDto> Roles { get; set; } = new List<RoleInfoDto>();

    }
}
