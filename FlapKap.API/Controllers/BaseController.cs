using FlapKap.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlapKap.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }
        protected ContentResult Success<T>(T result, int? totalCount = null, string messageCode = "Done Successfully", string message = null)
        {
            var responseMessage = message;
            var response = new SuccessResponse<T>(result, totalCount, responseMessage);
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            string json = JsonConvert.SerializeObject(response, settings);
            return Content(json, "application/json");
        }
    }
}
