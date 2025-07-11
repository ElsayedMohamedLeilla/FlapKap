using FlapKap.Models.DTOs.Products;
using FlapKap.Models.Requests;
using FlapKap.Models.Response.Products;

namespace FlapKap.Contract.BusinessLogic
{
    public interface IProductBL
    {
        Task<int> Create(CreateProductModel model);
        Task<bool> Update(UpdateProductModel model);
        Task<GetProductByIdResponseModel> GetById(int userId);
        Task<GetProductsResponse> Get(GetProductsCriteria model);
        Task<bool> Delete(int userId);


    }
}
