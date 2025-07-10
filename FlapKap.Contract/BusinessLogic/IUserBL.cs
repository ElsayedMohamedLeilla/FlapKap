using FlapKap.Models.DTOs.Users;
using FlapKap.Models.Requests;
using FlapKap.Models.Response.UserManagement;

namespace FlapKap.Contract.BusinessLogic
{
    public interface IUserBL
    {
        Task<int> Create(CreateUserModel model);
        Task<bool> Update(UpdateUserModel model);
        Task<GetUserByIdResponseModel> GetById(int userId);
        Task<GetUsersResponse> Get(GetUsersCriteria model);
        Task<bool> Delete(int userId);


    }
}
