using FlapKap.Contract.BusinessValidation;
using FlapKap.Contract.Repository;
using FlapKap.Models.Context;
using FlapKap.Models.DTOs.Exceptions;
using FlapKap.Models.DTOs.Users;
using Microsoft.EntityFrameworkCore;

namespace FlapKap.Validation.BusinessValidation
{

    public class UserBLValidation : IUserBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public UserBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateUserModel model)
        {
            var checkUserDuplicate = await repositoryManager
                .UserRepository.Get(c => c.UserName == model.UserName).AnyAsync();
            if (checkUserDuplicate)
            {
                throw new BusinessValidationException("Sorry User Name Is Duplicated");
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateUserModel model)
        {
            var checkUserDuplicate = await repositoryManager
                .UserRepository.Get(c => c.Id != model.Id &&
                c.UserName == model.UserName).AnyAsync();
            if (checkUserDuplicate)
            {
                throw new BusinessValidationException("Sorry User Name Is Duplicated");
            }

            return true;
        }
    }
}
