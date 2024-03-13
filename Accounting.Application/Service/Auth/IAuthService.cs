using Accounting.Application.Service.Auth.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Auth
{
    public interface IAuthService: IApplicationService
    {
        ServiceResponse<TokenInfo>GenerateToken(List<Claim> claims);
    }
}
