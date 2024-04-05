using Accounting.Application.Service.Management.Dtos;
using AccountingsTracker.Common.Dtos;

namespace Accounting.Application.Service.Management
{
    public interface IManagementService : IApplicationService
    {
        Task<ServiceResponse> DeleteUser(Guid id);
        Task<ServiceResponse<PagedResponseDto<UserListDto>>> GetAllUsers(GetAllUserRequestDto request);
        Task<ServiceResponse> UpdateUser(UserUpdateRequestDto request);
        Task<ServiceResponse<List<string>>> GetPermissionsRequests();
    }
}