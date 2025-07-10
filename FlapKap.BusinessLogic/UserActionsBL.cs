using FlapKap.Contract.BusinessLogic;
using FlapKap.Contract.BusinessValidation;
using FlapKap.Contract.Repository;
using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Models.Context;
using FlapKap.Models.DTOs.Users;
using FlapKap.Models.Response.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlapKap.BusinessLogic
{
    public class UserActionsBL : IUserActionsBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IUserActionsBLValidation userActionsBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly ILogger<UserActionsBL> _logger;
        public UserActionsBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
           RequestInfo _requestHeaderContext, ILogger<UserActionsBL> logger,
           IUserActionsBLValidation _userActionsBLValidation)
        {
            unitOfWork = _unitOfWork;
            _logger = logger;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            userActionsBLValidation = _userActionsBLValidation;
        }
        public async Task<bool> Deposit(DepositModel model)
        {

            #region Business Validation

            userActionsBLValidation.DepositValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Deposit

            var getUser = await repositoryManager.UserRepository.GetByIdAsync(requestInfo.UserId);
            if (getUser != null)
            {
                getUser.Deposit = (getUser.Deposit ?? 0) + model.DepositAmount;
                await unitOfWork.SaveAsync();
            }

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();

            _logger.LogInformation("User Deposit Done");
            return true;

            #endregion

        }
        public async Task<BuyResponseModel> Buy(BuyModel model)
        {
            var result = new BuyResponseModel();

            #region Business Validation

            var validationResult = await userActionsBLValidation.BuyValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Handle Update Deposit 

            var getUser = await repositoryManager.UserRepository.
                GetByIdAsync(requestInfo.UserId);
            if (getUser != null)
            {
                getUser.Deposit = (getUser.Deposit ?? 0) - validationResult.AllItemsCost;
                await unitOfWork.SaveAsync();
            }

            #region Hanlde Change

            var remDeposit = getUser.Deposit;
            result.TotalRemaining = getUser.Deposit ?? 0;

            while (remDeposit >= 5)
            {
                if (remDeposit >= 100)
                {
                    result.ChangeList.Add(100);
                    remDeposit -= 100;
                }
                else if (remDeposit >= 50)
                {
                    result.ChangeList.Add(50);
                    remDeposit -= 50;
                }
                else if (remDeposit >= 20)
                {
                    result.ChangeList.Add(20);
                    remDeposit -= 20;
                }
                else if (remDeposit >= 10)
                {
                    result.ChangeList.Add(10);
                    remDeposit -= 10;
                }
                else if (remDeposit >= 5)
                {
                    result.ChangeList.Add(5);
                    remDeposit -= 5;
                }
            }

            #endregion

            #endregion

            #region Handle Update Products

            var productIds = validationResult.DBItems.Select(d => d.Id).ToList();
            var getProducts = await repositoryManager.ProductRepository.
                GetWithTracking(p => productIds.Contains(p.Id)).ToListAsync();

            foreach (var product in getProducts)
            {
                var getItem = model.Items.FirstOrDefault(i => i.ProductId == product.Id);
                product.Quantity -= getItem?.Quantity ?? 0;
            }

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();

            var items = model.Items;
            result.TotalSpent = validationResult.AllItemsCost;
            result.Products = getProducts.
                Select(product => new BuyResponseItemModel
                {
                    ProductName = product.Name,
                    Quantity = items.FirstOrDefault(i => i.ProductId == product.Id).Quantity,
                    UnitPrice = product.Cost,
                    Total = product.Cost * items.FirstOrDefault(i => i.ProductId == product.Id).Quantity
                }).ToList();

            _logger.LogInformation("Buy Done");

            return result;

            #endregion

        }
        public async Task<bool> Reset()
        {

            #region Business Validation

            userActionsBLValidation.ResetValidation();

            #endregion

            unitOfWork.CreateTransaction();

            #region Reset

            var getUser = await repositoryManager.UserRepository.GetByIdAsync(requestInfo.UserId);
            if (getUser != null)
            {
                getUser.Deposit = null;
                await unitOfWork.SaveAsync();
            }

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            _logger.LogInformation("User Reset Done");
            return true;

            #endregion

        }
    }
}

