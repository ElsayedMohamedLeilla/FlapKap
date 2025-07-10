using FlapKap.Contract.BusinessLogic;
using FlapKap.Contract.BusinessValidation;
using FlapKap.Contract.Repository;
using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Domain.Entities.UserActions;
using FlapKap.Helpers;
using FlapKap.Models.Context;
using FlapKap.Models.DTOs.Exceptions;
using FlapKap.Models.DTOs.Users;
using FlapKap.Models.Requests;
using FlapKap.Models.Response.UserManagement;
using FlapKap.Validation.FluentValidation.Users;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FlapKap.BusinessLogic
{
    public class UserActionsBL : IUserActionsBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IUserActionsBLValidation userActionsBLValidation;
        private readonly IRepositoryManager repositoryManager;
        public UserActionsBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
           RequestInfo _requestHeaderContext,
           IUserActionsBLValidation _userActionsBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            userActionsBLValidation = _userActionsBLValidation;
        }
        public async Task<bool> Deposit(DepositModel model)
        {

            #region Business Validation

            await userActionsBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert UserActions

            var currentUserId = requestInfo.UserId;

            var userActions = new UserActions
            {
                Name = model.Name,
                Cost = model.Cost,
                SellerId = currentUserId,
                AddUserId = currentUserId,
                Quantity = model.Quantity
            };

            var createUserActionsResponse = repositoryManager.UserActionsRepository.
                Insert(userActions);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return userActions.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateUserActionsModel model)
        {

            #region Business Validation

            await userActionsBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update UserActions

            var currentUserId = requestInfo.UserId;
            var getUserActions = await repositoryManager.UserActionsRepository.
                GetEntityByConditionWithTrackingAsync(userActions =>
                userActions.Id == model.Id && userActions.SellerId == currentUserId) ??
                 throw new BusinessValidationException("Sorry UserActions Not Found");

            getUserActions.Name = model.Name;
            getUserActions.ModifiedDate = DateTime.UtcNow;
            getUserActions.ModifyUserId = currentUserId;
            getUserActions.Cost = model.Cost;
            getUserActions.Quantity = model.Quantity;

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetUserActionssResponse> Get(GetUserActionssCriteria criteria)
        {
            var userActionsRepository = repositoryManager.UserActionsRepository;
            var freeText = string.IsNullOrEmpty(criteria.FreeText) ?
                null : criteria.FreeText.Trim();
            var currentUserId = requestInfo.UserId;

            var query = userActionsRepository.Get(u => 
            (requestInfo.UserRole == UserRoleEnum.Buyer || u.SellerId == currentUserId) &&
            (freeText == null || u.Name.Contains(freeText)));

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = query.OrderByDescending(u => u.Id);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var userActionssList = await queryPaged.Select(userActions => new GetUserActionssResponseModel
            {
                Id = userActions.Id,
                Name = userActions.Name,
                Cost = userActions.Cost,
                Quantity = userActions.Quantity
            }).ToListAsync();

            return new GetUserActionssResponse
            {
                UserActionss = userActionssList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetUserActionsByIdResponseModel> GetById(int userActionsId)
        {
            var currentUserId = requestInfo.UserId;
            var userActions = await repositoryManager.UserActionsRepository.
                Get(userActions => userActions.Id == userActionsId && userActions.SellerId == currentUserId)
                .Select(userActions => new GetUserActionsByIdResponseModel
                {
                    Id = userActions.Id,
                    Name = userActions.Name,
                    Cost = userActions.Cost,
                    Quantity = userActions.Quantity
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException("Sorry UserActions Not Found");

            return userActions;

        }
        public async Task<bool> Delete(int userActionsId)
        {
            var currentUserId = requestInfo.UserId;
            var userActions = await repositoryManager.UserActionsRepository.
                GetEntityByConditionWithTrackingAsync(userActions => userActions.Id == userActionsId &&
                userActions.SellerId == currentUserId) ??
                throw new BusinessValidationException("Sorry UserActions Not Found");
            
            userActions.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}

