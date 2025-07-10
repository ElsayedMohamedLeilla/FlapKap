using FlapKap.Models.DTOs.Users;

namespace FlapKap.Contract.BusinessValidation
{
    public interface IUserBLValidation
    {
        Task<bool> CreateValidation(CreateUserModel model);
        Task<bool> UpdateValidation(UpdateUserModel model);
    }
}
