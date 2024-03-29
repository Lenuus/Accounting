using Accounting.Application.Service.Collection;
using Accounting.Application.Service.Collection.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }
        [Authorize(Policy = "EmployeeAndManagementPolicy")]
        [HttpPost("Create-Collection")]
        public async Task<IActionResult> CreateCollection([FromBody]CreateCollectionRequestDto request)
        {
            var response = await _collectionService.CreateCollection(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = "ManagementPolicy")]
        [HttpPost("Delete-Collection")]
        public async Task<IActionResult> DeleteCollection([FromBody] Guid id)
        {
            var response = await _collectionService.DeleteCollection(id).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = "EmployeeAndManagementPolicy")]
        [HttpPost("GetAll-Collection")]
        public async Task<IActionResult> GetAllCollection([FromBody] GetAllCollectionRequest request)
        {
            var response = await _collectionService.GetAllCollections(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = "EmployeeAndManagementPolicy")]
        [HttpPost("Update-Collection")]
        public async Task<IActionResult> UpdateCollection([FromBody] UpdateCollectionRequestDto request)
        {
            var response = await _collectionService.UpdateCollection(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = "ManagementPolicy")]
        [HttpPost("Delete-CollectionDocument")]
        public async Task<IActionResult> DeleteCollectionDocument([FromBody] Guid id)
        {
            var response = await _collectionService.DeleteCollectionDocument(id).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
