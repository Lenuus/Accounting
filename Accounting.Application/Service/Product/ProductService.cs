using Accounting.Application.Service.Product.Dtos;
using Accounting.Common.Enum;
using Accounting.Common.Helpers;
using Accounting.Domain;
using Accounting.Common.Dtos;
using Accounting.Common.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Domain.Product> _productRepository;
        private readonly IRepository<Domain.ProductImage> _productImageRepository;
        private readonly IMapper _mapper;
        private readonly IClaimManager _claimManager;

        public ProductService(IRepository<ProductImage> productImageRepository, IRepository<Domain.Product> productRepository, IMapper mapper, IClaimManager claimManager)
        {
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _claimManager = claimManager;
        }

        public async Task<ServiceResponse> CreateProduct(ProductCreateRequestDto request)
        {
            if (request == null)
            {
                new ServiceResponse(false, "Request is not valid");
            }
            var entity = _mapper.Map<Domain.Product>(request);
            if (request.Images != null && request.Images.Any())
            {
                var order = 0;
                foreach (var image in request.Images)
                {
                    if (image.Length > 0)
                    {
                        var newFileName = Guid.NewGuid().ToString().Replace("-", "");
                        newFileName += Path.GetExtension(image.FileName);
                        var newFilePath = Path.Combine(Path.GetFullPath("wwwroot/images"), newFileName);

                        using (var stream = System.IO.File.Create(newFilePath))
                        {
                            await image.CopyToAsync(stream).ConfigureAwait(false);
                        }
                        order++;
                        entity.Images.Add(new Domain.ProductImage()
                        {
                            Name = newFileName,
                            Path = newFilePath,
                            DisplayOrder = order
                        });
                    }
                }
            }
            entity.Id = Guid.NewGuid();
            entity.TenantId = _claimManager.GetTenantId();
            var properties = request.Properties.Select(f => new ProductPropertyDto
            {
                Name = f.Name,
                Value = f.Value,
            });

            foreach (var property in properties)
            {
                entity.Properties.Add(new ProductProperty
                {
                    ProductId = entity.Id,
                    Name = property.Name,
                    Value = property.Value
                });
            }

            await _productRepository.Create(entity).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> DeleteProduct(Guid id)
        {
            var product = await _productRepository.GetById(id).ConfigureAwait(false);
            if (product == null)
            {
                return new ServiceResponse(false, "Cannot Found");
            }
            await _productRepository.DeleteById(id).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> UpdateProduct(ProductUpdateRequestDto request)
        {
            var product = await _productRepository.GetAll()
               .Include(f => f.Properties)
               .Include(f => f.Images)
               .FirstOrDefaultAsync(f => f.Id == request.Id).ConfigureAwait(false);
            product.Id = request.Id;
            product.Number = request.Number;
            product.PurchasePrice = request.PurchasePrice;
            product.SellingPrice = request.SellingPrice;
            product.Barcode = request.Barcode;
            product.CurrentStock = request.CurrentStock;
            product.Tax = request.Tax;
            var updatedProperties = new List<ProductProperty>();

            foreach (var property in request.Properties)
            {
                var existingProperty = product.Properties.FirstOrDefault(p => p.Name == property.Name);
                if (existingProperty != null)
                {
                    existingProperty.Value = property.Value;
                    updatedProperties.Add(existingProperty);
                }
                else
                {
                    updatedProperties.Add(new ProductProperty
                    {
                        Name = property.Name,
                        Value = property.Value
                    });
                }

            }

            if (!request.NewImages.IsNullOrEmpty())
            {
                int lastOrder = 0;
                var existingImages = _productImageRepository.GetAll().Where(f => f.ProductId == request.Id);
                if (existingImages.Any())
                {
                    lastOrder = existingImages.Max(f => f.DisplayOrder);
                }

                foreach (var image in request.NewImages)
                {
                    if (image.Length > 0)
                    {

                        if (image.Length > 0)
                        {
                            var newFileName = Guid.NewGuid().ToString().Replace("-", "");
                            newFileName += Path.GetExtension(image.FileName);
                            var newFilePath = Path.Combine(Path.GetFullPath("wwwroot/images"), newFileName);

                            using (var stream = System.IO.File.Create(newFilePath))
                            {
                                await image.CopyToAsync(stream).ConfigureAwait(false);
                            }
                            lastOrder++;
                            product.Images.Add(new Domain.ProductImage()
                            {
                                Name = newFileName,
                                Path = newFilePath,
                                DisplayOrder = lastOrder
                            });
                        }
                    }
                }
            }
            await _productRepository.Update(product).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse<PagedResponseDto<ProductDetailDto>>> GetAllProduct(GetAllProductRequestDto request)
        {
            var loggedTenantId = _claimManager.GetTenantId();
            var query = _productRepository.GetAll()
                .Include(f => f.Images)
                .Include(f => f.Properties)
                .Where(f => !f.IsDeleted && f.TenantId == loggedTenantId &&
                    (!string.IsNullOrEmpty(request.Number) ? f.Number.Contains(request.Number) : true) &&
                    (!string.IsNullOrEmpty(request.Barcode) ? f.Barcode.Contains(request.Barcode) : true) &&
                    (request.Stock.HasValue ? f.CurrentStock == request.Stock : true) &&
                    (request.SellingPrice.HasValue ? f.SellingPrice == request.SellingPrice : true) &&
                    (request.PurchasePrice.HasValue ? f.PurchasePrice == request.PurchasePrice : true));

            // Sıralama
            if (request.OrderBySelect == ProductOrderBy.SellingPrice)
            {
                if (request.SortDirection == SortDirection.Ascending)
                    query = query.OrderBy(f => f.SellingPrice);
                else
                    query = query.OrderByDescending(f => f.SellingPrice);
            }
            else if (request.OrderBySelect == ProductOrderBy.PurchasePrice)
            {
                if (request.SortDirection == SortDirection.Ascending)
                    query = query.OrderBy(f => f.PurchasePrice);
                else
                    query = query.OrderByDescending(f => f.PurchasePrice);
            }
            else if (request.OrderBySelect == ProductOrderBy.Stock)
            {
                if (request.SortDirection == SortDirection.Ascending)
                    query = query.OrderBy(f => f.CurrentStock);
                else
                    query = query.OrderByDescending(f => f.CurrentStock);
            }

            var productList = await query.Select(f => new ProductDetailDto
            {
                Id = f.Id,
                Barcode = f.Barcode,
                SellingPrice = f.SellingPrice,
                PurchasePrice = f.PurchasePrice,
                CurrentStock = f.CurrentStock,
                Number = f.Number,
                Tax = f.Tax,
                Images = f.Images.Where(d => !d.IsDeleted).OrderBy(d => d.DisplayOrder).Select(i => new ProductImageDto
                {
                    Path = i.Path
                }).ToList(),
                Properties = f.Properties.Where(d => !d.IsDeleted).Select(p => new ProductPropertyDto
                {
                    Name = p.Name,
                    Value = p.Value,
                }).ToList(),
            }).ToPagedListAsync(request.PageSize, request.PageIndex).ConfigureAwait(false);

            if (productList == null)
            {
                return new ServiceResponse<PagedResponseDto<ProductDetailDto>>(null, false, "Ürünlerin listesi getirilemedi");
            }
            return new ServiceResponse<PagedResponseDto<ProductDetailDto>>(productList, true, string.Empty);
        }


    }
}

