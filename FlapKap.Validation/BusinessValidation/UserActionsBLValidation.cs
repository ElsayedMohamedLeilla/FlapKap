using FlapKap.Contract.BusinessValidation;
using FlapKap.Contract.Repository;
using FlapKap.Models.Context;
using FlapKap.Models.DTOs.Exceptions;
using FlapKap.Models.DTOs.Users;
using Microsoft.EntityFrameworkCore;

namespace FlapKap.Validation.BusinessValidation
{

    public class UserActionsBLValidation : IUserActionsBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public UserActionsBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public bool DepositValidation(DepositModel model)
        {
            // if there is depoit limit or some thing like that

            return true;
        }
        public bool ResetValidation()
        {
            var getUser = requestInfo.User;

            if (getUser == null)
            {
                throw new BusinessValidationException("Sorry User Not Found");
            }
            if (getUser.Deposit == null || getUser.Deposit == 0)
            {
                throw new BusinessValidationException("Sorry There Is No Deposit To Reset");
            }

            return true;
        }
        public async Task<bool> BuyValidation(BuyModel model)
        {
            var items = model.Items;
            var productIds = model.Items.Select(x => x.ProductId).ToList();

            var checkIfAllProductsIsCorrect = await repositoryManager
                .ProductRepository.
                Get(c => !productIds.Contains(c.Id)).AnyAsync();

            if (checkIfAllProductsIsCorrect)
            {
                throw new BusinessValidationException("Sorry Some Products Not Correct");
            }

            var getDBProducts = await repositoryManager
                .ProductRepository.Get(p => productIds.Contains(p.Id)).
                Select(p => new
                {
                    p.Name,
                    p.Id,
                    p.Quantity,
                    p.Cost
                }).ToListAsync();

            var checkQuantity = getDBProducts.FirstOrDefault(d => d.Quantity <
            items.FirstOrDefault(i => i.ProductId == d.Id).Quantity);

            if (checkQuantity != null)
            {
                throw new BusinessValidationException("Sorry There Is No Available Quantity For Product: " +
                    checkQuantity.Name + " - Available Quantity= " + checkQuantity.Quantity);
            }

            var totals = items.Select(i => new
            {
                total = i.Quantity * getDBProducts.First(d => d.Id == i.ProductId).Cost,
            }).ToList();

            var sumAllItems = totals.Sum(t => t.total);

            var getUser = requestInfo.User;

            if (sumAllItems > getUser.Deposit)
            {
                var sumRounded = Math.Round(sumAllItems, 2).ToString();
                var depositRounded = Math.Round(getUser.Deposit ?? 0, 2).ToString();

                throw new BusinessValidationException("Sorry Your Deposit Is Less Than Required Items. Items Cost Is: " +
                    sumRounded + " Your Deposit: " + depositRounded);
            }

            return true;
        }
    }
}
