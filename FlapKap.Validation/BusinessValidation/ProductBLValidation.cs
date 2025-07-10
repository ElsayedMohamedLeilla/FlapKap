using FlapKap.Contract.BusinessValidation;
using FlapKap.Contract.Repository;
using FlapKap.Models.Context;
using FlapKap.Models.DTOs.Exceptions;
using FlapKap.Models.DTOs.Users;
using Microsoft.EntityFrameworkCore;

namespace FlapKap.Validation.BusinessValidation
{

    public class ProductBLValidation : IProductBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public ProductBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateProductModel model)
        {
            var checkProductDuplicate = await repositoryManager
                .ProductRepository.Get(c => c.Name == model.Name &&
                c.SellerId == requestInfo.UserId).AnyAsync();
            if (checkProductDuplicate)
            {
                throw new BusinessValidationException("Sorry Product Name Is Duplicated");
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateProductModel model)
        {
            var checkProductDuplicate = await repositoryManager
                .ProductRepository.Get(c => c.Id != model.Id &&
                c.Name == model.Name &&
                c.SellerId == requestInfo.UserId).AnyAsync();

            if (checkProductDuplicate)
            {
                throw new BusinessValidationException("Sorry Product Name Is Duplicated");
            }

            return true;
        }
    }
}
