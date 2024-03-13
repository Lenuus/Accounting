using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Account.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public DateTime Expire { get; set; }
    }
}
