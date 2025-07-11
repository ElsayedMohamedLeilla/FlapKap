using FlapKap.Contract.BusinessLogic;
using FlapKap.Contract.BusinessValidation;
using FlapKap.Contract.Repository;
using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Domain.Entities.Product;
using FlapKap.Helpers;
using FlapKap.Models.Context;
using FlapKap.Models.DTOs.Exceptions;
using FlapKap.Models.DTOs.Products;
using FlapKap.Models.DTOs.Users;
using FlapKap.Models.Requests;
using FlapKap.Models.Response.Products;
using FlapKap.Models.Response.UserManagement;
using FlapKap.Validation.FluentValidation.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace FlapKap.BusinessLogic
{
    public class ProductBL : IProductBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IProductBLValidation productBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly ILogger<ProductBL> _logger;
        public ProductBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
           RequestInfo _requestHeaderContext, ILogger<ProductBL> logger,
           IProductBLValidation _productBLValidation)
        {
            unitOfWork = _unitOfWork;
            _logger = logger;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            productBLValidation = _productBLValidation;
        }
        public async Task<int> Create(CreateProductModel model)
        {

            #region Business Validation

            await productBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Product

            var currentUserId = requestInfo.UserId;

            var product = new Product
            {
                Name = model.Name,
                Cost = model.Cost,
                SellerId = currentUserId,
                AddUserId = currentUserId,
                Quantity = model.Quantity
            };

            var createProductResponse = repositoryManager.ProductRepository.
                Insert(product);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            _logger.LogInformation("Create Product Done");
            return product.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateProductModel model)
        {

            #region Business Validation

            await productBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Product

            var currentUserId = requestInfo.UserId;
            var getProduct = await repositoryManager.ProductRepository.
                GetEntityByConditionWithTrackingAsync(product =>
                product.Id == model.Id && product.SellerId == currentUserId) ??
                 throw new BusinessValidationException("Sorry Product Not Found");

            getProduct.Name = model.Name;
            getProduct.ModifiedDate = DateTime.UtcNow;
            getProduct.ModifyUserId = currentUserId;
            getProduct.Cost = model.Cost;
            getProduct.Quantity = model.Quantity;

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            _logger.LogInformation("Update Product Done");
            return true;

            #endregion
        }
        public async Task<GetProductsResponse> Get(GetProductsCriteria criteria)
        {
            var productRepository = repositoryManager.ProductRepository;
            var freeText = string.IsNullOrEmpty(criteria.FreeText) ?
                null : criteria.FreeText.Trim();
            var currentUserId = requestInfo.UserId;

            var query = productRepository.Get(u => 
            (requestInfo.UserRole == UserRoleEnum.Buyer || u.SellerId == currentUserId) &&
            (freeText == null || u.Name.Contains(freeText)));

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = query.OrderByDescending(u => u.Id);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var productsList = await queryPaged.Select(product => new GetProductsResponseModel
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Quantity = product.Quantity
            }).ToListAsync();

            return new GetProductsResponse
            {
                Products = productsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetProductByIdResponseModel> GetById(int productId)
        {
            var currentUserId = requestInfo.UserId;
            var product = await repositoryManager.ProductRepository.
                Get(product => product.Id == productId && product.SellerId == currentUserId)
                .Select(product => new GetProductByIdResponseModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Cost = product.Cost,
                    Quantity = product.Quantity
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException("Sorry Product Not Found");

            return product;

        }
        public async Task<bool> Delete(int productId)
        {
            var currentUserId = requestInfo.UserId;
            var product = await repositoryManager.ProductRepository.
                GetEntityByConditionWithTrackingAsync(product => product.Id == productId &&
                product.SellerId == currentUserId) ??
                throw new BusinessValidationException("Sorry Product Not Found");
            
            product.Delete();
            await unitOfWork.SaveAsync();
            _logger.LogInformation("Delete Product Done");
            return true;
        }
    }
}

