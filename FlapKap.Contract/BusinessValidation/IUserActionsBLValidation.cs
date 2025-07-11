using FlapKap.Models.DTOs.UserActions;
using FlapKap.Models.Response.UserActions;

namespace FlapKap.Contract.BusinessValidation
{
    public interface IUserActionsBLValidation
    {
        bool DepositValidation(DepositModel model);
        Task<GetBuyValidationResponseModel> BuyValidation(BuyModel model);
        bool ResetValidation();
    }
}
