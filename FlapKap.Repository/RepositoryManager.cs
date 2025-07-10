using FlapKap.Contract.Repository;
using FlapKap.Contract.Repository.UserManagement;
using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Models.Context;
using FlapKap.Repository.UserManagement;

namespace FlapKap.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly ApplicationDBContext context;
        private readonly RequestInfo requestInfo;
        private IUserRepository userRepository;
        private IUserTokenRepository userTokenRepository;
        private IUserRoleRepository userRoleRepository;
        private IProductRepository productRepository;



        public RepositoryManager(IUnitOfWork<ApplicationDBContext> _unitOfWork, RequestInfo _requestInfo)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestInfo;
        }



        public IUserRepository UserRepository =>
         userRepository ??= new UserRepository(requestInfo, unitOfWork);

        public IUserTokenRepository UserTokenRepository =>
         userTokenRepository ??= new UserTokenRepository(unitOfWork);
        public IUserRoleRepository UserRoleRepository =>
         userRoleRepository ??= new UserRoleRepository(unitOfWork);
        public IProductRepository ProductRepository =>
        productRepository ??= new ProductRepository(requestInfo, unitOfWork);

    }
}
