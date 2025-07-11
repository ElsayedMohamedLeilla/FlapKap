using FlapKap.Models.DTOs.UserActions;
using FlapKap.Models.Response.UserManagement;

namespace FlapKap.Contract.BusinessLogic
{
    public interface IUserActionsBL
    {
        Task<bool> Deposit(DepositModel model);
        Task<BuyResponseModel> Buy(BuyModel model);
        Task<bool> Reset();
    }
}
