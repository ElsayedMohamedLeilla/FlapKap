using FlapKap.Models.DTOs.Users;
using FlapKap.Models.Response.UserManagement;

namespace FlapKap.Contract.BusinessValidation
{
    public interface IUserActionsBLValidation
    {
        bool DepositValidation(DepositModel model);
        Task<GetBuyValidationResponseModel> BuyValidation(BuyModel model);
        bool ResetValidation();
    }
}
