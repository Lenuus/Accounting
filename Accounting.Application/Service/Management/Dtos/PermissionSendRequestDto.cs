using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Management.Dtos
{
    public class PermissionSendRequestDto
    {
        public List<string> Permissions { get; set; }= new List<string>();
    }
}
