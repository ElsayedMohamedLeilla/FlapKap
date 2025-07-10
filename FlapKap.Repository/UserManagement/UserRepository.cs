using FlapKap.Contract.Repository.UserManagement;
using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Domain.Entities.User;
using FlapKap.Models.Context;

namespace FlapKap.Repository.UserManagement
{
    public class UserRepository : GenericRepository<MyUser>, IUserRepository
    {
        private readonly RequestInfo requestInfo;
        private ApplicationDBContext Context { get; set; }

        public UserRepository(RequestInfo _requestInfo, IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
        {
            requestInfo = _requestInfo;
            Context = unitOfWork.Context;
        }
    }

}
