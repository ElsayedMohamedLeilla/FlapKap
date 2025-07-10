using FlapKap.Contract.Repository.UserManagement;

namespace FlapKap.Contract.Repository
{
    public interface IRepositoryManager
    {
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IUserTokenRepository UserTokenRepository { get; }
    }
}