using FlapKap.Models.DTOs;
using FlapKap.Models.Requests;

namespace FlapKap.Contract.BusinessLogic
{
    public interface IAuthenticationBL
    {
        Task<TokenDto> SignIn(SignInModel model);
    }
}
