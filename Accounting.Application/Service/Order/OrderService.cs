using Accounting.Application.Service.Corporation.Dtos;
using Accounting.Application.Service.Order.Dtos;
using Accounting.Application.Service.Product.Dtos;
using Accounting.Common.Enum;
using Accounting.Common.Helpers;
using Accounting.Domain;
using AccountingsTracker.Common.Dtos;
using AccountingsTracker.Common.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Accounting.Application.Service.Order
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Domain.Order> _orderRepository;
        private readonly IMapper _mapper;
        private readonly IClaimManager _claimManager;
        private readonly IRepository<Domain.CorporationRecord> _corporationRecordRepository;
        private readonly IRepository<Domain.Product> _productRepository;
        private readonly IRepository<Domain.ProductOrder> _productOrderRepository;
        private readonly IRepository<Domain.ProductRecord> _productRecordRepository;

        public OrderService(IRepository<Domain.Order> orderRepository, IMapper mapper, IClaimManager claimManager, IRepository<Domain.CorporationRecord> corporationRecordRepository, IRepository<Domain.Product> productRepository, IRepository<ProductOrder> productOrderRepository, IRepository<ProductRecord> productRecordRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _claimManager = claimManager;
            _corporationRecordRepository = corporationRecordRepository;
            _productRepository = productRepository;
            _productOrderRepository = productOrderRepository;
            _productRecordRepository = productRecordRepository;
        }

        public async Task<ServiceResponse> CreateOrder(OrderCreateRequestDto request)
        {
            if (request == null || request.Products == null || request.Products.Count == 0)
            {
                return new ServiceResponse(false, "Invalid request or no products in the request");
            }
            var mappedRequest = _mapper.Map<Domain.Order>(request);
            mappedRequest.Products = new List<ProductOrder>();
            mappedRequest.NetPrice = 0;
            mappedRequest.Id = Guid.NewGuid();


            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var productDto in request.Products)
                    {
                        var product = await _productRepository.GetById(productDto.Id).ConfigureAwait(false);
                        if (product == null)
                        {
                            return new ServiceResponse(false, $"Product with ID {product.Barcode} not found");
                        }

                        if (product.CurrentStock < productDto.Quantity)
                        {
                            return new ServiceResponse(false, $"Insufficient stock for product with ID {product.Barcode}");
                        }
                        #region TotalPrice
                        mappedRequest.NetPrice += productDto.Price * productDto.Quantity;// vergisiz fiyat
                        mappedRequest.TotalDiscount += (productDto.Price * (productDto.Discount / 100)) * productDto.Quantity;
                        mappedRequest.TotalTaxAmount += (productDto.Price * (product.Tax / 100)) * productDto.Quantity;
                        mappedRequest.TotalPrice += (mappedRequest.NetPrice - mappedRequest.TotalDiscount) + mappedRequest.TotalTaxAmount;
                        product.CurrentStock -= productDto.Quantity;
                        #endregion

                        mappedRequest.Products.Add(new ProductOrder
                        {
                            ProductId = productDto.Id,
                        });
                        product.ProductRecords.Add(new ProductRecord
                        {
                            ProductId = productDto.Id,
                            TenantId = _claimManager.GetTenantId(),
                            InOut = true,
                            Quantity = productDto.Quantity,
                            Price = productDto.Price,
                            Discount = productDto.Discount,
                            NetPrice = productDto.Quantity * productDto.Price,
                            TotalPrice = (productDto.Quantity * productDto.Price * (1 - (productDto.Discount / 100)) * (1 + product.Tax / 100)),
                            ReferenceId = mappedRequest.Id
                        });
                        await _productRepository.Update(product).ConfigureAwait(false);
                    }
                    mappedRequest.TenantId = _claimManager.GetTenantId();

                    var order = await _orderRepository.Create(mappedRequest).ConfigureAwait(false);
                    if (order == null)
                    {
                        foreach (var productDto in request.Products)
                        {
                            var product = await _productRepository.GetById(productDto.Id).ConfigureAwait(false);
                            product.CurrentStock += productDto.Quantity;
                            await _productRepository.Update(product).ConfigureAwait(false);
                        }

                        return new ServiceResponse(false, "Failed to create order");
                    }
                    var record = await OrderRecord(order).ConfigureAwait(false);
                    if (!record.IsSuccesfull)
                    {
                        return new ServiceResponse(false, "Record cannot be created");
                    }
                    scope.Complete();
                }
                catch (Exception)
                {

                    return new ServiceResponse(false, "Order could not Created");
                }
            }
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse<PagedResponseDto<OrderListDto>>> GetAllOrder(GetAllOrderRequestDto request)
        {
            var loggedUserId = _claimManager.GetUserId();
            var loggedTenantId = _claimManager.GetTenantId();

            var query = _orderRepository.GetAll()
               .Include(f => f.Products)
              .ThenInclude(f => f.Product)
              .Where(f => !f.IsDeleted && f.TenantId == loggedTenantId &&
                  (string.IsNullOrEmpty(request.Number) || EF.Functions.Like(EF.Functions.Collate(f.Number, "SQL_Latin1_General_CP1_CI_AS"), $"%{request.Number}%")) &&
                  ((request.StartDate == null || request.StartDate <= f.Date) && (request.EndDate == null || request.EndDate >= f.LastDate)));
            if (request.SortDirection == SortDirection.Descending)
            {
                query.OrderByDescending(f => f.Date);
            }
            if (request.SortDirection == SortDirection.Ascending)
            {
                query.OrderBy(f => f.Date);
            }

            var listedOrders = await query.Select(f => new OrderListDto
            {
                CorporationId = f.CorporationId,
                InOut = f.InOut,
                Date = f.Date,
                LastDate = f.LastDate,
                TotalPrice = f.TotalPrice,
                Tax = f.TotalTaxAmount,
                Number = f.Number,
                TotalDiscount = f.TotalDiscount,
                NetPrice = f.NetPrice,
                Products = f.Products.Where(p => !p.Product.IsDeleted).Select(p => new ProductDetailDto
                {
                    Barcode = p.Product.Barcode,
                    SellingPrice = p.Product.SellingPrice,
                    Number = p.Product.Number,
                    Images = p.Product.Images.Where(i => !i.IsDeleted).Select(i => new ProductImageDto
                    {
                        Path = i.Path
                    }).ToList()
                }).ToList()
            }).ToPagedListAsync(request.PageSize, request.PageIndex).ConfigureAwait(false);


            return new ServiceResponse<PagedResponseDto<OrderListDto>>(listedOrders, true, string.Empty);
        }

        public async Task<ServiceResponse> RemoveOrder(Guid id)
        {
            var order = await _orderRepository.GetById(id).ConfigureAwait(false);
            if (order == null)
            {
                return new ServiceResponse(false, "Cant found");
            }
            await _corporationRecordRepository.DeleteById(order.Id).ConfigureAwait(false);
            await _orderRepository.DeleteById(id).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        public async Task<ServiceResponse> UpdateOrder(UpdateOrderRequestDto request)
        {
            if (request == null)
            {
                return new ServiceResponse(false, "Request is not valid");
            }

            var order = await _orderRepository.GetById(request.Id).ConfigureAwait(false);
            if (order == null)
            {
                return new ServiceResponse(false, "Requested order cannot be found");
            }

            var originalOrder = CloneOrder(order);
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    order.TotalDiscount = 0;
                    order.NetPrice = 0;
                    order.TotalPrice = 0;
                    order.TotalTaxAmount = 0;
                    order.CorporationId = request.CorporationId;
                    order.LastDate = request.LastDate;
                    order.InOut = request.InOut;
                    order.Date = request.Date;
                    order.Number = request.Number;
                    new List<ProductOrder>();

                    foreach (var product in request.Products)
                    {

                        var currentProduct = await _productRepository.GetById(product.Id).ConfigureAwait(false);
                        var productRecord = _productRecordRepository.GetAll().FirstOrDefault(f => f.ReferenceId == order.Id && f.ProductId == currentProduct.Id);
                        var currentProductQuantity = productRecord.Quantity;
                        var newCurrentStock = currentProduct.CurrentStock += currentProductQuantity;
                        var updatedStock = newCurrentStock - product.Quantity;
                        if (updatedStock <= 0)
                        {
                            new ServiceResponse(false, $"Not enough stock for the product with  {currentProduct.Barcode} Barcode Number ");
                        }
                        currentProduct.CurrentStock = updatedStock;
                        var productTax = currentProduct.Tax;
                        order.NetPrice += product.Price * product.Quantity; // Vergisiz fiyat
                        order.TotalDiscount += (product.Price * (product.Discount / 100)) * product.Quantity;
                        order.TotalTaxAmount += (product.Price * (productTax / 100)) * product.Quantity;
                        order.TotalPrice += (order.NetPrice - order.TotalDiscount) + order.TotalTaxAmount;

                        productRecord.Discount = product.Discount;
                        productRecord.TotalPrice = order.TotalPrice;
                        productRecord.NetPrice = order.NetPrice;
                        productRecord.Price = product.Price;
                        productRecord.Quantity = product.Quantity;
                    }

                    await _orderRepository.Update(order).ConfigureAwait(false);
                    var updateRecord = await updateCorpRecord(order).ConfigureAwait(false);

                    if (!updateRecord.IsSuccesfull)
                    {
                        await _orderRepository.Update(originalOrder).ConfigureAwait(false);

                        return new ServiceResponse(false, "Corporation Record cannot be updated");
                    }
                    scope.Complete();
                }
                catch (Exception)
                {

                    return new ServiceResponse(false, "Order Update failed");
                }
            }
            return new ServiceResponse(true, string.Empty);
        }

        private Domain.Order CloneOrder(Domain.Order order)
        {
            return new Domain.Order
            {
                Id = order.Id,
                CorporationId = order.CorporationId,
                LastDate = order.LastDate,
                InOut = order.InOut,
                Date = order.Date,
                Number = order.Number,
                NetPrice = order.NetPrice,
                TotalDiscount = order.TotalDiscount,
                TotalTaxAmount = order.TotalTaxAmount,
                TotalPrice = order.TotalPrice
            };
        }
        private async Task<ServiceResponse> updateCorpRecord(Domain.Order order)
        {
            var orderCorpRecord = await _corporationRecordRepository.GetCorpRecordByReferenceId(order.Id).ConfigureAwait(false);
            if (orderCorpRecord == null)
            {
                return new ServiceResponse(false, "Order record cannot found");
            }
            orderCorpRecord.ReferenceId = order.Id;
            orderCorpRecord.CorporationId = order.CorporationId;
            orderCorpRecord.ActDate = order.Date;
            orderCorpRecord.LastDate = order.Date;
            orderCorpRecord.Price = order.TotalPrice;
            orderCorpRecord.InOut = order.InOut;
            await _corporationRecordRepository.Update(orderCorpRecord).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

        private async Task<ServiceResponse> OrderRecord(Domain.Order order)
        {
            var corpOrderRecord = new CorporationRecordCreateRequestDto
            {
                ActDate = order.Date,
                CorporationId = order.CorporationId,
                LastDate = order.LastDate,
                Price = order.TotalPrice,
                ActType = ActType.Order,
                InOut = order.InOut,
                TenantId = order.TenantId,
                ReferenceId = order.Id
            };
            var record = _mapper.Map<Domain.CorporationRecord>(corpOrderRecord);
            if (record == null)
            {
                return new ServiceResponse(false, "Record cannot created");
            }
            await _corporationRecordRepository.Create(record).ConfigureAwait(false);
            return new ServiceResponse(true, string.Empty);
        }

    }
}
