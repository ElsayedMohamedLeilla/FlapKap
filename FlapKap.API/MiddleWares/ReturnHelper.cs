using FlapKap.Data;
using FlapKap.Data.UnitOfWork;
using FlapKap.Models.DTOs.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlapKap.API.MiddleWares
{
    public static class ReturnHelper
    {
        public static async Task Return(IUnitOfWork<ApplicationDBContext> unitOfWork, HttpContext context, int statusCode, ErrorResponse response)
        {
            unitOfWork.Rollback();
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, settings));
            }
        }
    }
}
