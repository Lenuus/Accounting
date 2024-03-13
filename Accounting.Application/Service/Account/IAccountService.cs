using Accounting.Application.Service.Account.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Account
{
    public interface IAccountService: IApplicationService
    {
        Task<ServiceResponse<LoginResponseDto>> Login(LoginRequestDto request);
        Task<ServiceResponse> Register(RegisterRequestDto request);
    }
}
