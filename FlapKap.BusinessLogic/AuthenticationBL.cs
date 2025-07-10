using FlapKap.Contract.BusinessLogic;
using FlapKap.Contract.BusinessValidation;
using FlapKap.Contract.Repository;
using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Models;
using FlapKap.Models.Context;
using FlapKap.Models.DTOs;
using FlapKap.Models.Requests;
using FlapKap.Repository.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlapKap.BusinessLogic
{
    public class AuthenticationBL : IAuthenticationBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly UserManagerRepository userManagerRepository;
        private readonly Jwt jwt;
        private readonly RequestInfo requestHeaderContext;
        private readonly IHttpContextAccessor accessor;
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        private readonly ILogger<AuthenticationBL> _logger;
        private readonly IAuthenticationBLValidation authenticationBLValidation;
        public AuthenticationBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IAuthenticationBLValidation _authenticationBLValidation,
            IRepositoryManager _repositoryManager, ILogger<AuthenticationBL> logger,
            UserManagerRepository _userManagerRepository,
            IOptions<Jwt> _appSettings, RequestInfo _requestInfo,
           RequestInfo _userContext, IHttpContextAccessor _accessor)
        {
            unitOfWork = _unitOfWork;
            _logger = logger;
            authenticationBLValidation = _authenticationBLValidation;
            userManagerRepository = _userManagerRepository;
            requestHeaderContext = _userContext;
            jwt = _appSettings.Value;
            requestInfo = _requestInfo;
            repositoryManager = _repositoryManager;
            accessor = _accessor;
        }
        public async Task<TokenDto> SignIn(SignInModel model)
        {
            #region Business Validation

            var user = await authenticationBLValidation.SignInValidation(model);

            #endregion

            #region Get User Role

            var roles = await userManagerRepository.GetRolesAsync(user);

            #endregion

            #region Get Token Model

            TokenModel tokenModelSearchCriteria = new()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles
            };

            var tokenData = GetTokenModel(tokenModelSearchCriteria);

            #endregion
            _logger.LogInformation("User " + user.UserName + " Logged In Successfully");
            return tokenData;
        }
        private TokenDto GetTokenModel(TokenModel criteria)
        {
            #region Create Token

            ClaimsIdentity claimsIdentity = new(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, criteria.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, criteria.UserId.ToString()),
                new Claim("UserId", criteria.UserId.ToString())
            });
            if (criteria.Roles != null)
            {
                claimsIdentity.AddClaims(criteria.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            JwtSecurityTokenHandler tokenHandler = new();

            var key = Encoding.ASCII.GetBytes(jwt.Key);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwt.Issuer,
                Audience = jwt.Issuer
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            var tokenData = FormateToken(criteria.UserId, token);

            #endregion

            return tokenData;
        }
        private TokenDto FormateToken(int? userId, string token)
        {
            #region Get Token

            TokenDto tokenModel = new()
            {
                Token = token,
                UserId = userId ?? 0
            };

            #endregion

            return tokenModel;
        }
    }
}

