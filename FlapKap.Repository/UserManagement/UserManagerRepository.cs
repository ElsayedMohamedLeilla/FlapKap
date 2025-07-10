using FlapKap.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlapKap.Repository.UserManagement
{
    public class GlameraUserIdentityOptions : IdentityOptions
    {

    }
    public sealed class UserManagerRepository : UserManager<MyUser>
    {

        public UserManagerRepository(IUserStore<MyUser> store, IOptions<IdentityOptions> optionsAccessor,
       IPasswordHasher<MyUser> passwordHasher, IEnumerable<IUserValidator<MyUser>> userValidators,
       IEnumerable<IPasswordValidator<MyUser>> passwordValidators, ILookupNormalizer keyNormalizer,
       IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<MyUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }
        public override Task<string> GeneratePasswordResetTokenAsync(MyUser user)
        {
            ThrowIfDisposed();
            return GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, ResetPasswordTokenPurpose);
        }

        public override async Task<string> GenerateUserTokenAsync(MyUser user, string tokenProvider, string purpose)
        {
            return await base.GenerateUserTokenAsync(user, tokenProvider, purpose);
        }

    }
}
