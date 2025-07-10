using FlapKap.Domain.Entities.User;
using FlapKap.Models.Requests;

namespace FlapKap.Contract.BusinessValidation
{
    public interface IAuthenticationBLValidation
    {
        Task<MyUser> SignInValidation(SignInModel model);
    }
}
