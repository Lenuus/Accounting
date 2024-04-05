using Accounting.Application.Service.Collection;
using Accounting.Application.Service.Collection.Dtos;
using Accounting.Common.Constants;
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
        [Authorize(Policy = RoleClaimConstants.CollectionAdd)]
        [HttpPost("create-collection")]
        public async Task<IActionResult> CreateCollection([FromBody]CollectionCreateRequestDto request)
        {
            var response = await _collectionService.CreateCollection(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = RoleClaimConstants.CollectionDelete)]
        [HttpPost("delete-collection")]
        public async Task<IActionResult> DeleteCollection([FromBody] Guid id)
        {
            var response = await _collectionService.DeleteCollection(id).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = RoleClaimConstants.CollectionList)]
        [HttpPost("get-all-collections")]
        public async Task<IActionResult> GetAllCollection([FromBody] GetAllCollectionRequest request)
        {
            var response = await _collectionService.GetAllCollections(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = RoleClaimConstants.CollectionUpdate)]
        [HttpPost("update-collection")]
        public async Task<IActionResult> UpdateCollection([FromBody] CollectionUpdateRequestDto request)
        {
            var response = await _collectionService.UpdateCollection(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize(Policy = RoleClaimConstants.CollectionUpdate)]
        [HttpPost("delete-collection-document")]
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
