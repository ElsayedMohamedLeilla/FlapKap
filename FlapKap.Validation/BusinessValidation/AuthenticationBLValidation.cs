using FlapKap.Contract.BusinessValidation;
using FlapKap.Contract.Repository;
using FlapKap.Domain.Entities.User;
using FlapKap.Models.DTOs.Exceptions;
using FlapKap.Models.Requests;
using FlapKap.Repository.UserManagement;

namespace FlapKap.Validation.BusinessValidation
{

    public class AuthenticationBLValidation : IAuthenticationBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly UserManagerRepository userManagerRepository;
        public AuthenticationBLValidation(IRepositoryManager _repositoryManager,
            UserManagerRepository _userManagerRepository)
        {
            repositoryManager = _repositoryManager;
            userManagerRepository = _userManagerRepository;
        }
        public async Task<MyUser> SignInValidation(SignInModel model)
        {
            #region Try Find User

            var user = await repositoryManager.UserRepository
                .GetEntityByConditionAsync(u => !u.IsDeleted && u.UserName == model.UserName) ??
                throw new BusinessValidationException("Sorry User Not Found");

            #endregion

            #region Check Password

            bool checkPasswordAsyncRes = await userManagerRepository.CheckPasswordAsync(user, model.Password);
            if (!checkPasswordAsyncRes)
            {
                throw new BusinessValidationException("Sorry Password Not Correct");
            }

            #endregion

            return user;
        }
    }
}
