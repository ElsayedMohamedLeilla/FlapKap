using FlapKap.Models.DTOs;

namespace FlapKap.Models.Response
{
    public class SignInResponse : BaseResponse
    {
        public TokenDto TokeObject { get; set; }
    }
}
