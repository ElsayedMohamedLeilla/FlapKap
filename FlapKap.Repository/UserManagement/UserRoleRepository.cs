using FlapKap.Contract.Repository.UserManagement;
using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Domain.Entities.User;

namespace FlapKap.Repository.UserManagement
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
        {

        }
    }

}
