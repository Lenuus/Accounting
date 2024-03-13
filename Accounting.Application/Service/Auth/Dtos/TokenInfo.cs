using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Auth.Dtos
{
    public class TokenInfo
    {
        public string Token  { get; set; }
        public DateTime Expire { get; set; }
    }
}
