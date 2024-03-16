using Accounting.Application.Service.Account.Dtos;
using Accounting.Application.Service.Corporation;
using Accounting.Application.Service.Corporation.Dtos;
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

        [HttpPost("Create-Corporation")]
        [Authorize(Policy = "CreateCorporation")]
        public async Task<IActionResult> CreateCorporation([FromBody] CorporationRegisterRequestDto request)
        {
            var response = await _corporationService.CreateCorporation(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("GetAll-Corporation")]
        [Authorize]
        public async Task<IActionResult> GetAllCorporation([FromBody] GetAllCorporationRequest request)
        {
            var corporations=await _corporationService.GetAllCorporations(request).ConfigureAwait(false);
            if (!corporations.IsSuccesfull)
            {
                return BadRequest(corporations);
            }
            return Ok(corporations);

        }
    }
}
