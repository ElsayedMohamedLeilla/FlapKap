using FlapKap.Contract.BusinessLogic;
using FlapKap.Models.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlapKap.API.Controllers
{
    [Route("api/[controller]/[action]"), ApiController]
    [Authorize(Roles = "Buyer")]
    public class ActionController : BaseController
    {
        private readonly IUserActionsBL userActionsBL;

        public ActionController(IUserActionsBL _userActionsBL)
        {
            userActionsBL = _userActionsBL;
        }
        [HttpPut]
        public async Task<ActionResult> Deposit([FromBody] DepositModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await userActionsBL.Deposit(model);
            return Success(result, message: "Done Deposit Successfully");
        }
        [HttpPut]
        public async Task<ActionResult> Buy([FromBody] BuyModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await userActionsBL.Buy(model);
            return Success(result, message: "Done Buy Successfully");
        }
        [HttpPut]
        public async Task<ActionResult> Reset()
        {
            var result = await userActionsBL.Reset();
            return Success(result, message: "Done Reset Successfully");
        }
    }
}