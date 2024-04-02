using Accounting.Application.Service.Collection.Dtos;
using Accounting.Application.Service.Management;
using Accounting.Application.Service.Management.Dtos;
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

        [Authorize(Policy = "ManagementRolePolicy")]
        [HttpPost("Delete-User")]
        public async Task<IActionResult> DeleteUser([FromBody] Guid id)
        {
            var response = await _managementService.DeleteUser(id).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = "ManagementRolePolicy")]
        [HttpPost("GetAll-User")]
        public async Task<IActionResult> GetAllUser([FromBody] GetAllUserRequestDto request)
        {
            var response = await _managementService.GetAllUsers(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = "ManagementRolePolicy")]
        [HttpPost("Update-User")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequestDto request)
        {
            var response = await _managementService.UpdateUser(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
