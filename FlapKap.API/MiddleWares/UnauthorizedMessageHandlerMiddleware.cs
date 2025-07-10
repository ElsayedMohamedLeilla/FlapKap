using FlapKap.API.MiddleWares;
using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Models.Context;
using FlapKap.Models.DTOs.Exceptions;
using FlapKap.Models.Response;

namespace Dawem.API.MiddleWares
{
    public class UnauthorizedMessageHandlerMiddleware
    {
        private readonly RequestDelegate _request;

        public UnauthorizedMessageHandlerMiddleware(RequestDelegate next)
        {
            _request = next;
        }
        public Task Invoke(HttpContext context, RequestInfo userContext, IUnitOfWork<ApplicationDBContext> unitOfWork) => InvokeAsync(context, userContext, unitOfWork);
        async Task InvokeAsync(HttpContext context, RequestInfo requestInfo, IUnitOfWork<ApplicationDBContext> unitOfWork)
        {
            await _request.Invoke(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized && !context.Response.HasStarted)
            {
                int statusCode = StatusCodes.Status401Unauthorized;
                var response = new ErrorResponse
                {
                    State = ResponseStatus.UnAuthorized,
                    Message = "Sorry Your Access Data Is Not Correct Please Check Your User Name And Password"
                };
                await ReturnHelper.Return(unitOfWork, context, statusCode, response);

            }
            else if ((context.Response.StatusCode == StatusCodes.Status403Forbidden) && !context.Response.HasStarted)
            {
                int statusCode = StatusCodes.Status403Forbidden;
                var response = new ErrorResponse
                {
                    State = ResponseStatus.Forbidden,
                    Message = "Sorry You Are Forbidden To Access Requested Data"
                };
                await ReturnHelper.Return(unitOfWork, context, statusCode, response);

            }
        }

    }
}