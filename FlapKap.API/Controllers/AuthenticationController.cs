using FlapKap.Contract.BusinessLogic;
using FlapKap.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlapKap.API.Controllers
{
    [Route("api/[controller]/[action]"), ApiController]
    [Authorize]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationBL authenticationBL;

        public AuthenticationController(IAuthenticationBL _authenticationBL)
        {
            authenticationBL = _authenticationBL;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(SignInModel signInModel)
        {
            return Success(await authenticationBL.SignIn(signInModel), message: "Done Sign You In Successfully");
        }
    }
}