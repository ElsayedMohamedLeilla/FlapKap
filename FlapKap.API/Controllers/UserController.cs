using FlapKap.Contract.BusinessLogic;
using FlapKap.Models.DTOs.Users;
using FlapKap.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlapKap.API.Controllers
{
    [Route("api/[controller]/[action]"), ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserBL userBL;

        public UserController(IUserBL _userBL)
        {
            userBL = _userBL;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Create([FromBody] CreateUserModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await userBL.Create(model);
            return Success(result, message: "Done Create User Successfully");
        }
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateUserModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await userBL.Update(model);
            return Success(result, message: "Done Update User Successfully");
        }
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetUsersCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest();
            }
            var usersresponse = await userBL.Get(criteria);

            return Success(usersresponse.Users, usersresponse.TotalCount);
        }
        [HttpGet]
        public async Task<ActionResult> GetById([FromQuery] int userId)
        {
            if (userId < 1)
            {
                return BadRequest();
            }
            return Success(await userBL.GetById(userId));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int userId)
        {
            if (userId < 1)
            {
                return BadRequest();
            }
            return Success(await userBL.Delete(userId));
        }
    }
}