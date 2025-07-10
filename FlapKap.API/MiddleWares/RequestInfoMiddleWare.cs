using FlapKap.Contract.Repository;
using FlapKap.Helpers;
using FlapKap.Models.Context;
using FlapKap.Models.DTOs.Users;
using FlapKap.Repository.UserManagement;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace FlapKap.API.MiddleWares
{
    public class RequestInfoMiddleWare
    {
        private readonly RequestDelegate _next;

        public RequestInfoMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, RequestInfo requestInfo,
            UserManagerRepository userManager, IRepositoryManager repositoryManager)
        {
            requestInfo.Lang = HttpRequestHelper.getLangKey(httpContext.Request);
            requestInfo.Lang = requestInfo.Lang == null || requestInfo.Lang.Length > 2 ? "ar" : requestInfo.Lang;

            int userId = 0;

            try
            {
                string token = httpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(token))
                {
                    var tok = token.Replace("Bearer ", "");
                    var jwttoken = new JwtSecurityTokenHandler().ReadJwtToken(tok);

                    var userIdText = jwttoken.Claims.First(claim => claim.Type == "UserId")?.Value ?? "";

                    _ = int.TryParse(userIdText.ToString(), out userId);

                    requestInfo.UserId = userId;

                }

            }
            catch (Exception ex)
            {
                // do nothing if jwt validation fails
            }

            if (userId > 0)
            {
                var userIdString = userId.ToString();
                requestInfo.User = await userManager.FindByIdAsync(userIdString);

                #region Roles

                if (requestInfo.User != null)
                {
                    var userRoles = await userManager.GetRolesAsync(requestInfo.User);
                    if (userRoles != null && userRoles.Count > 0)
                    {
                        var role = userRoles.FirstOrDefault();
                        requestInfo.UserRole = role == "Seller" ?
                            UserRoleEnum.Seller : UserRoleEnum.Buyer;
                    }
                }
              
                #endregion
            }
            if (Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith("ar"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
            }

            await _next.Invoke(httpContext);


        }

    }
}
