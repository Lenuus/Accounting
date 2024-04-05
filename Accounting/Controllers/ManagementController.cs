using Accounting.Application.Service.Collection.Dtos;
using Accounting.Application.Service.Management;
using Accounting.Application.Service.Management.Dtos;
using Accounting.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;

namespace Accounting.Controllers
{
    public class ManagementController : Controller
    {
        private readonly IManagementService _managementService;

        public ManagementController(IManagementService managementService)
        {
            _managementService = managementService;
        }

        [Authorize(Policy = RoleClaimConstants.ManagementDelete)]
        [HttpPost("delete-user")]
        public async Task<IActionResult> DeleteUser([FromBody] Guid id)
        {
            var response = await _managementService.DeleteUser(id).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = RoleClaimConstants.ManagementList)]
        [HttpPost("get-all-user")]
        public async Task<IActionResult> GetAllUser([FromBody] GetAllUserRequestDto request)
        {
            var response = await _managementService.GetAllUsers(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = RoleClaimConstants.ManagementUpdate)]
        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequestDto request)
        {
            var response = await _managementService.UpdateUser(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("get-all-perms")]
        public async Task<IActionResult> GetAllPerms()
        {
            var response = await _managementService.GetPermissionsRequests().ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
