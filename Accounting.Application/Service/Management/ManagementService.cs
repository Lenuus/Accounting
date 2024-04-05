using Accounting.Application.Service.Management.Dtos;
using Accounting.Common.Constants;
using Accounting.Common.CustomAttribute;
using Accounting.Common.Helpers;
using Accounting.Domain;
using AccountingsTracker.Common.Dtos;
using AccountingsTracker.Common.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Accounting.Application.Service.Management
{
    public class ManagementService : IManagementService
    {
        private readonly IRepository<Domain.User> _userRepository;
        private readonly IRepository<Domain.UserRole> _userRoleRepository;
        private readonly IRepository<Domain.UserClaim> _userClaimRepository;
        private readonly UserManager<Domain.User> _userManager;


        public ManagementService(IRepository<UserRole> useRoleRepository,
            IRepository<User> userRepository,
            IRepository<UserClaim> userClaimRepository,
            UserManager<User> userManager)
        {
            _userRoleRepository = useRoleRepository;
            _userRepository = userRepository;
            _userClaimRepository = userClaimRepository;
            _userManager = userManager;
        }

        public async Task<ServiceResponse> DeleteUser(Guid id)
        {
            var user = await _userRepository.GetById(id).ConfigureAwait(false);
            if (user == null)
            {
                return new ServiceResponse(false, "User could not found");
            }
            await _userRepository.Delete(user).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse<PagedResponseDto<UserListDto>>> GetAllUsers(GetAllUserRequestDto request)
        {

            var result = await _userRepository.GetAll().Include(f => f.Roles).ThenInclude(f => f.Role)
                                  .Where(f => !f.IsDeleted &&
                                    (!request.Search.IsNullOrEmpty() ?
                                       f.UserName.Contains(request.Search) ||
                                       f.Email.Contains(request.Search) ||
                                       f.PhoneNumber.Contains(request.Search) : true) &&
                                    (request.Roles.Any() ?
                                       f.Roles.Any(r => request.Roles.Contains(r.RoleId)) : true)).Select(f => new UserListDto
                                       {
                                           UserName = f.UserName,
                                           Email = f.Email,
                                           Roles = f.Roles.Select(x => new RoleInfoDto
                                           {
                                               Id = x.Role.Id,
                                               Name = x.Role.Name
                                           }).ToList(),
                                       }).ToPagedListAsync(request.PageSize, request.PageIndex).ConfigureAwait(false);

            return new ServiceResponse<PagedResponseDto<UserListDto>>(result, true, string.Empty);
        }

        public async Task<ServiceResponse> UpdateUser(UserUpdateRequestDto request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                var user = await _userRepository.GetAll()
                               .Include(f => f.Roles)
                               .ThenInclude(f => f.Role).Include(f => f.Claims)
                               .FirstOrDefaultAsync(f => f.Id == request.Id).ConfigureAwait(false);
                if (user == null)
                {
                    return new ServiceResponse(false, "User not found");
                }

                user.PhoneNumber = request.PhoneNumber;
                user.Email = request.Email;
                user.NormalizedEmail = request.Email.ToUpper();
                //_userManager.AddClaimAsync(user, new System.Security.Claims.Claim(RoleClaimConstants.ClaimTypeValue, ""));
                //var test = await _userManager.GetClaimsAsync(user);
                //_userManager.RemoveClaimsAsync(user, test.Where(f => f.Type == RoleClaimConstants.ClaimTypeValue));
                if (request.Roles != null && request.Roles.Any())
                {
                    var rolesToAddIds = request.Roles.Where(f => !user.Roles.Any(y => f == y.RoleId));
                    var rolesToRemove = user.Roles.Where(f => !request.Roles.Contains(f.RoleId)).ToList();

                    foreach (var roleId in rolesToAddIds)
                    {
                        var userRole = new UserRole { RoleId = roleId, UserId = user.Id };
                        user.Roles.Add(userRole);
                    }

                    if (rolesToRemove.Any())
                    {
                        foreach (var deletedItem in rolesToRemove)
                        {
                            user.Roles.Remove(deletedItem);
                            await _userRoleRepository.Delete(deletedItem).ConfigureAwait(false);
                        }
                    }

                }
                if (request.Claims != null && request.Claims.Any())
                {
                    var claimsToAddStr = request.Claims.Where(f => !user.Claims.Any(y => f == y.ClaimValue && y.ClaimType == RoleClaimConstants.ClaimTypeValue));
                    var claimsToRemove = user.Claims.Where(f => !request.Claims.Contains(f.ClaimValue) && f.ClaimType == RoleClaimConstants.ClaimTypeValue).ToList();

                    foreach (var claim in claimsToAddStr)
                    {
                        var userClaim = new UserClaim { ClaimValue = claim, ClaimType = RoleClaimConstants.ClaimTypeValue };
                        user.Claims.Add(userClaim);
                    }

                    if (claimsToRemove.Any())
                    {
                        foreach (var deletedItem in claimsToRemove)
                        {
                            user.Claims.Remove(deletedItem);
                            await _userClaimRepository.Delete(deletedItem).ConfigureAwait(false);
                        }
                    }

                }
                await _userRepository.Update(user).ConfigureAwait(false);
                scope.Complete();
                return new ServiceResponse(true, "User updated successfully");

            }
        }

        public async Task<ServiceResponse<List<string>>> GetPermissionsRequests()
        {
            var groupedPermissions = GetGroupedPermissions();
            var permissions = new List<string>();

            foreach (var permissionGroup in groupedPermissions.Values)
            {
                permissions.AddRange(permissionGroup);
            }

            return new ServiceResponse<List<string>>(permissions, true, string.Empty);
        }

        public static Dictionary<string, List<string>> GetGroupedPermissions()
        {
            Dictionary<string, List<string>> _dict = new Dictionary<string, List<string>>();

            var fields = typeof(RoleClaimConstants).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (var field in fields)
            {
                string groupName = "Other";

                var groupAttribute = field.GetCustomAttribute<GroupAttribute>();
                if (groupAttribute != null)
                {
                    groupName = groupAttribute.GroupName;
                }

                if (!_dict.ContainsKey(groupName))
                {
                    _dict.Add(groupName, new List<string>());
                }

                string fieldValue = field.GetValue(null)?.ToString();

                _dict[groupName].Add(fieldValue);
            }

            return _dict;
        }



    }

}


