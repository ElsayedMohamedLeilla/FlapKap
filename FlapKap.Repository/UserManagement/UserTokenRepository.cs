using FlapKap.Contract.Repository.UserManagement;
using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Domain.Entities.User;

namespace FlapKap.Repository.UserManagement
{
    public class UserTokenRepository : GenericRepository<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
        {

        }
    }

}
