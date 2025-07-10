using FlapKap.Models.DTOs.Users;

namespace FlapKap.Contract.BusinessValidation
{
    public interface IUserActionsBLValidation
    {
        bool DepositValidation(DepositModel model);
        Task<bool> BuyValidation(BuyModel model);
        bool ResetValidation();
    }
}
