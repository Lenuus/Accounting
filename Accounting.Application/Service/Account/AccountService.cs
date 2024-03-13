using Accounting.Application.Service.Account.Dtos;
using Accounting.Application.Service.Auth;
using Accounting.Domain;
using AccountingsTracker.Common.Constants;
using AccountingsTracker.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Account
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Domain.User> _userRepository;
        private readonly PasswordHelper _passwordHelper;
        private readonly IRepository<Domain.Role> _roleRepository;
        private readonly IAuthService _authService;

        public AccountService(IRepository<User> userRepository, IRepository<Role> roleRepository, PasswordHelper passwordHelper, IAuthService authService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHelper = passwordHelper;
            _authService = authService;
        }

        public async Task<ServiceResponse<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var user = await _userRepository.GetAll().
                                            Include(f => f.Roles).ThenInclude(f => f.Role)
                                            .FirstOrDefaultAsync(f => f.Email == request.Email).ConfigureAwait(false);
            if (user == null)
            {
                return new ServiceResponse<LoginResponseDto>(null, false, "User not found");
            }

            if (user.IsDeleted)
            {
                return new ServiceResponse<LoginResponseDto>(null, false, "User not found");
            }

            if (!_passwordHelper.Verify(request.Password, user.PasswordHash))
            {
                return new ServiceResponse<LoginResponseDto>(null, false, "Email or password is incorrect");
            }

            if (!user.EmailConfirmed)
            {
                return new ServiceResponse<LoginResponseDto>(null, false, "Activate your email");
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtTokenConstants.UserId, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.Email.ToString()));
            var roleName = user.Roles.FirstOrDefault().Role.Name;
            claims.Add(new Claim(ClaimTypes.Role, roleName));
            var tokenInfo = _authService.GenerateToken(claims);
            if (!tokenInfo.IsSuccesfull)
            {
                return new ServiceResponse<LoginResponseDto>(null, false, "Token cannot be created");
            }
            var loginInfo = new LoginResponseDto()
            {
                Expire = tokenInfo.Data.Expire,
                Token = tokenInfo.Data.Token,
            };
            return new ServiceResponse<LoginResponseDto>(loginInfo, true, string.Empty);
        }
        public async Task<ServiceResponse> Register(RegisterRequestDto request)
        {
            if (!EmailIsValid(request.Email))
            {
                return new ServiceResponse(false, "Email is not correct address");
            }

            if (request.Email != request.EmailConfirmation)
            {
                return new ServiceResponse(false, "Email and email confirmation are not same");
            }

            if (request.Password != request.PasswordConfirmation)
            {
                return new ServiceResponse(false, "Password and password confirmation are not same");
            }

            if (!_passwordHelper.PasswordValid(request.Password))
            {
                return new ServiceResponse(false, "Password is weak");
            }

            var normalizedEmail = request.Email.ToNormalize();
            var userExists = await _userRepository.GetAll().FirstOrDefaultAsync(f => f.NormalizedEmail == normalizedEmail && !f.IsDeleted).ConfigureAwait(false);
            if (userExists != null)
            {
                return new ServiceResponse(false, "User already exists");
            }

            var userRole = await _roleRepository.GetAll().FirstOrDefaultAsync(f => f.Name == RoleConstants.User).ConfigureAwait(false);
            if (userRole == null)
            {
                return new ServiceResponse(false, "User role not exists");
            }

            var user = new Domain.User
            {
                Email = request.Email,
                NormalizedEmail = normalizedEmail,
                UserName = request.Email,
                NormalizedUserName = normalizedEmail,
                PasswordHash = _passwordHelper.Hash(request.Password),
                Roles = new List<Domain.UserRole>
                {
                    new Domain.UserRole{ RoleId = userRole.Id }
                }
            };
            await _userRepository.Create(user);
            return new ServiceResponse(true, string.Empty);
        }
        private bool EmailIsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
