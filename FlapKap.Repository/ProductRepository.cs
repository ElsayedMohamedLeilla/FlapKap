using FlapKap.Contract.Repository;
using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Domain.Entities.Product;
using FlapKap.Models.Context;

namespace FlapKap.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly RequestInfo requestInfo;
        private ApplicationDBContext Context { get; set; }

        public ProductRepository(RequestInfo _requestInfo, IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
        {
            requestInfo = _requestInfo;
            Context = unitOfWork.Context;
        }
    }

}
