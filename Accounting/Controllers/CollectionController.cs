using Accounting.Application.Service.Collection;
using Accounting.Application.Service.Collection.Dtos;
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

        [HttpPost("Create-Collection")]
        public async Task<IActionResult> CreateCollection(CreateCollectionRequestDto request)
        {
            var response = await _collectionService.CreateCollection(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Delete-Collection")]
        public async Task<IActionResult> DeleteCollection(Guid id)
        {
            var response = await _collectionService.DeleteCollection(id).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("GetAll-Collection")]
        public async Task<IActionResult> GetAllCollection(GetAllCollectionRequest request)
        {
            var response = await _collectionService.GetAllCollections(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Update-Collection")]
        public async Task<IActionResult> UpdateCollection(UpdateCollectionRequestDto request)
        {
            var response = await _collectionService.UpdateCollection(request).ConfigureAwait(false);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("Delete-CollectionDocument")]
        public async Task<IActionResult> DeleteCollectionDocument(Guid id)
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
