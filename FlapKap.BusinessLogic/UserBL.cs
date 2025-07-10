using FlapKap.Contract.BusinessLogic;
using FlapKap.Contract.BusinessValidation;
using FlapKap.Contract.Repository;
using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Domain.Entities.User;
using FlapKap.Helpers;
using FlapKap.Models.Context;
using FlapKap.Models.DTOs.Exceptions;
using FlapKap.Models.DTOs.Users;
using FlapKap.Models.Requests;
using FlapKap.Models.Response.UserManagement;
using FlapKap.Repository.UserManagement;
using FlapKap.Validation.FluentValidation;
using FlapKap.Validation.FluentValidation.Users;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FlapKap.BusinessLogic
{
    public class UserBL : IUserBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IUserBLValidation userBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly UserManagerRepository userManagerRepository;
        public UserBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            UserManagerRepository _userManagerRepository,
           RequestInfo _requestHeaderContext,
           IUserBLValidation _userBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            userBLValidation = _userBLValidation;
            userManagerRepository = _userManagerRepository;
        }
        public async Task<int> Create(CreateUserModel model)
        {
            #region Business Validation

            await userBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert User

            var user = new MyUser
            {
                UserName = model.UserName,
                Email = model.UserName.Contains("@") ? model.UserName : 
                model.UserName + "@flapkap.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };

            var createUserResponse = await userManagerRepository.
                CreateAsync(user, model.Password);
            if (!createUserResponse.Succeeded)
            {
                unitOfWork.Rollback();
                var error = createUserResponse.Errors.FirstOrDefault();
                if (error != null)
                {
                    throw new BusinessValidationException(error.Code + " " + error.Description);
                }
                else
                {
                    throw new BusinessValidationException("Sorry Error Happen When Create User");
                }
            }

            #region Roles

            var role = model.Role == UserRoleEnum.Seller ?
                   ["Seller"] : new List<string> { "Buyer" };
            await userManagerRepository.AddToRolesAsync(user, role);

            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return user.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateUserModel model)
        {

            #region Business Validation

            await userBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update User

            var getUser = await repositoryManager.UserRepository.
                GetEntityByConditionWithTrackingAsync(user =>
                user.Id == model.Id);

            getUser.UserName = model.UserName;
            getUser.ModifiedDate = DateTime.Now;
            getUser.ModifyUserId = requestInfo.UserId;

            var updateUserResponse = await userManagerRepository.UpdateAsync(getUser);

            if (!updateUserResponse.Succeeded)
            {
                await unitOfWork.RollbackAsync();
                throw new BusinessValidationException("Sorry Error Happen While Updating User");
            }

            await unitOfWork.SaveAsync();

            #region Roles

            var role = model.Role == UserRoleEnum.Seller ?
                ["Seller"] : new List<string> { "Buyer" };

            bool isInRole = await userManagerRepository.IsInRoleAsync(getUser, role.First());

            if (!isInRole)
            {
                var currentRoles = await userManagerRepository.GetRolesAsync(getUser);

                await userManagerRepository.RemoveFromRolesAsync(getUser, currentRoles);
                await userManagerRepository.AddToRolesAsync(getUser, role);
            }

            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetUsersResponse> Get(GetUsersCriteria criteria)
        {
            var userRepository = repositoryManager.UserRepository;
            var freeText = string.IsNullOrEmpty(criteria.FreeText) ? 
                null : criteria.FreeText.Trim() ;

            var query = userRepository.Get(u => freeText == null ||
            u.UserName.Contains(freeText));

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = query.OrderByDescending(u => u.Id);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var usersList = await queryPaged.Select(user => new GeUsersResponseModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Deposit = user.Deposit,
                Role = user.UserRoles.FirstOrDefault().Role.Name
            }).ToListAsync();

            return new GetUsersResponse
            {
                Users = usersList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetUserByIdResponseModel> GetById(int userId)
        {
            var user = await repositoryManager.UserRepository.
                Get(user => user.Id == userId)
                .Select(user => new GetUserByIdResponseModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Role = user.UserRoles.FirstOrDefault().Role.Name == "Seller" ?
                    UserRoleEnum.Seller : UserRoleEnum.Buyer
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException("Sorry User Not Found");

            return user;

        }
        public async Task<bool> Delete(int userId)
        {
            var user = await repositoryManager.UserRepository.
                GetEntityByConditionWithTrackingAsync(user => user.Id == userId) ??
                throw new BusinessValidationException("Sorry User Not Found");

            #region Roles

            var userRoles = await userManagerRepository.GetRolesAsync(user);
            if (userRoles != null && userRoles.Count > 0)
            {
                var role = userRoles.FirstOrDefault();
                var roleEnum = role == "Seller" ?
                    UserRoleEnum.Seller : UserRoleEnum.Buyer;

                if (requestInfo.UserRole != roleEnum)
                {
                    throw new BusinessValidationException("Sorry You Can Not Delete User Diff In Your role");
                }
            }

            #endregion

            user.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

