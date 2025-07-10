using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Models.DTOs.Exceptions;
using FlapKap.Models.Response;
using System.Net;

namespace FlapKap.API.MiddleWares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _request;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _request = next;
            _logger = logger;
        }
        public Task Invoke(HttpContext context, IUnitOfWork<ApplicationDBContext> unitOfWork) => InvokeAsync(context, unitOfWork);
        async Task InvokeAsync(HttpContext context, IUnitOfWork<ApplicationDBContext> unitOfWork)
        {
            int statusCode = 500;
            var response = new ErrorResponse();

            try
            {
                await _request.Invoke(context);
            }
            catch (BusinessValidationException exception)
            {
                unitOfWork.Rollback();
                _logger.LogError(exception.Message);
                statusCode = (int)HttpStatusCode.UnprocessableEntity;
                response.State = ResponseStatus.ValidationError;
                response.Message = exception.Message;
                await ReturnHelper.Return(unitOfWork, context, statusCode, response);
            }
            catch (Exception exception)
            {
                unitOfWork.Rollback();
                _logger.LogError(exception.Message);
                statusCode = (int)HttpStatusCode.InternalServerError;
                response.State = ResponseStatus.Error;
                response.Message = exception.Message;
                await ReturnHelper.Return(unitOfWork, context, statusCode, response);
            }
        }
    }
}