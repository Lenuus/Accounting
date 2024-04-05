using Accounting.Application.Service.Account.Dtos;
using Accounting.Application.Service.Corporation;
using Accounting.Application.Service.Corporation.Dtos;
using Accounting.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Controllers
{
    public class CorporationController : Controller
    {
        private readonly ICorporationService _corporationService;

        public CorporationController(ICorporationService corporationService)
        {
            _corporationService = corporationService;
        }
        #region Corporation
        [Authorize(Policy = RoleClaimConstants.CorporationAdd)]
        [HttpPost("create-corporation")]
        public async Task<IActionResult> CreateCorporation([FromBody] CorporationRegisterRequestDto request)
        {
            var response = await _corporationService.CreateCorporation(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = RoleClaimConstants.CorporationUpdate)]
        [HttpPost("get-all-corporations")]
        public async Task<IActionResult> GetAllCorporation([FromBody] GetAllCorporationRequestDto request)
        {
            var corporations = await _corporationService.GetAllCorporations(request).ConfigureAwait(false);
            if (!corporations.IsSuccesfull)
            {
                return BadRequest(corporations);
            }
            return Ok(corporations);
        }

        [Authorize(Policy = RoleClaimConstants.CorporationDelete)]
        [HttpPost("delete-corporation")]
        public async Task<IActionResult> DeleteCorporation([FromBody] Guid id)
        {
            var response = await _corporationService.RemoveCorporation(id).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = RoleClaimConstants.CorporationUpdate)]
        [HttpPost("update-corporation")]
        public async Task<IActionResult> UpdateCorporation([FromBody] CorporationUpdateRequestDto request)
        {
            var response = await _corporationService.UpdateCorporation(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        #endregion


        #region CorporationRecord

        [Authorize(Policy = RoleClaimConstants.CorporationAdd)]
        [HttpPost("create-corporation-record")]
        public async Task<IActionResult> CreateCorporationRecord([FromBody] CorporationRecordCreateRequestDto request)
        {
            var response = await _corporationService.CreateCorporationRecord(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = RoleClaimConstants.CorporationDelete)]
        [HttpPost("delete-corporation-record")]
        public async Task<IActionResult> DeleteCorporationRecord([FromBody] Guid id)
        {
            var response = await _corporationService.RemoveCorporationRecord(id).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        #endregion
    }
}
