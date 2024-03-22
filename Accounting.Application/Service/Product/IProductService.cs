using Accounting.Application.Service.Order.Dtos;
using Accounting.Application.Service.Product.Dtos;
using AccountingsTracker.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Product
{
    public interface IProductService : IApplicationService
    {
        Task<ServiceResponse> CreateProduct(ProductCreateRequestDto request);
        Task<ServiceResponse> DeleteProduct(Guid id);
        Task<ServiceResponse> UpdateProduct(ProductUpdateRequestDto request);
        Task<ServiceResponse<PagedResponseDto<ProductDetailDto>>> GetAllProduct(GetAllProductRequestDto request);

    }
}
