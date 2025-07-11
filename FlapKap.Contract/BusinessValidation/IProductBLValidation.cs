using FlapKap.Models.DTOs.Products;

namespace FlapKap.Contract.BusinessValidation
{
    public interface IProductBLValidation
    {
        Task<bool> CreateValidation(CreateProductModel model);
        Task<bool> UpdateValidation(UpdateProductModel model);
    }
}
