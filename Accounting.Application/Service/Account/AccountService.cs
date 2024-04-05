using Accounting.Application.Service.Account.Dtos;
using Accounting.Application.Service.Auth;
using Accounting.Domain;
using AccountingsTracker.Common.Constants;
using AccountingsTracker.Common.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly SignInManager<Domain.User> _singInManager;
        private readonly UserManager<Domain.User> _userManager;
        private readonly IRepository<Domain.UserClaim> _userClaimRepository;
        private readonly IRepository<Domain.RoleClaim> _roleClaimRepository;
        private readonly IMemoryCache _memoryCache;

        public AccountService(
            IRepository<User> userRepository, 
            IRepository<Role> roleRepository, 
            PasswordHelper passwordHelper, 
            IAuthService authService, 
            UserManager<User> userManager, 
            SignInManager<User> singInManager, 
            IRepository<UserClaim> userClaimRepository, 
            IRepository<RoleClaim> roleClaimRepository, 
            IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHelper = passwordHelper;
            _authService = authService;
            _userManager = userManager;
            _singInManager = singInManager;
            _userClaimRepository = userClaimRepository;
            _roleClaimRepository = roleClaimRepository;
            _memoryCache = memoryCache;
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


            if (!user.EmailConfirmed)
            {
                return new ServiceResponse<LoginResponseDto>(null, false, "Activate your email");
            }
            var signInResult = await _singInManager.PasswordSignInAsync(user, request.Password, true, true).ConfigureAwait(false);
            if (!signInResult.Succeeded)
            {
                return new ServiceResponse<LoginResponseDto>(null, false, "Email or password is wrong");
            }
            return await CreateTokenForUser(user).ConfigureAwait(false);
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

            var user = await _userManager.CreateAsync(new Domain.User { Email = request.Email, UserName = request.Email }, request.Password).ConfigureAwait(false);
            if (user.Succeeded)
            {
                var createdUser = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
                createdUser.TenantId = Guid.NewGuid();
                await _userManager.AddToRoleAsync(createdUser, RoleConstants.User).ConfigureAwait(false);
                return new ServiceResponse(true, string.Empty);
            }
            return new ServiceResponse(false, string.Join("n", user.Errors.Select(f => f.Description)));
        }
        private async Task<ServiceResponse<LoginResponseDto>> CreateTokenForUser(Domain.User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtTokenConstants.UserId, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(JwtTokenConstants.TenantId, user.TenantId.ToString()));

            var userClaims = new List<string>();
            var userDbClaims = await _userClaimRepository.GetAll().Where(f => f.UserId == user.Id).ToListAsync().ConfigureAwait(false);
            userClaims.AddRange(userDbClaims.Select(f => f.ClaimValue).Distinct());
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
                if (!userDbClaims.Any())
                {
                    var roleDbClaims = await _roleClaimRepository.GetAll().Where(f => f.RoleId == role.RoleId).ToListAsync().ConfigureAwait(false);
                    userClaims.AddRange(roleDbClaims.Select(f => f.ClaimValue).Distinct());
                }
            }
            _memoryCache.Set($"claims_{user.Id}", userClaims);

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
