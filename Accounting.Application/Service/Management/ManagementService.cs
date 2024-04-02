using Accounting.Application.Service.Management.Dtos;
using Accounting.Common.Helpers;
using Accounting.Domain;
using AccountingsTracker.Common.Dtos;
using AccountingsTracker.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Accounting.Application.Service.Management
{
    public class ManagementService : IManagementService
    {
        private readonly IRepository<Domain.User> _userRepository;
        private readonly IRepository<Domain.UserRole> _userRoleRepository;

        public ManagementService(IRepository<UserRole> useRoleRepository, IRepository<User> userRepository)
        {
            _userRoleRepository = useRoleRepository;
            _userRepository = userRepository;
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
                try
                {
                    var user = await _userRepository.GetAll()
                                   .Include(f => f.Roles)
                                   .ThenInclude(f => f.Role)
                                   .FirstOrDefaultAsync(f => f.Id == request.Id).ConfigureAwait(false);
                    if (user == null)
                    {
                        return new ServiceResponse(false, "User not found");
                    }

                    user.PhoneNumber = request.PhoneNumber;
                    user.Email = request.Email;
                    user.NormalizedEmail = request.Email.ToUpper();
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

                    await _userRepository.Update(user).ConfigureAwait(false);
                    scope.Complete();
                    return new ServiceResponse(true, "User updated successfully");
                }
                catch (Exception)
                {
                    return new ServiceResponse(false, "Could not update user");
                }
            }
        }

    }

}
