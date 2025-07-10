using FlapKap.Models.DTOs.Users;

namespace FlapKap.Contract.BusinessValidation
{
    public interface IProductBLValidation
    {
        Task<bool> CreateValidation(CreateProductModel model);
        Task<bool> UpdateValidation(UpdateProductModel model);
    }
}
