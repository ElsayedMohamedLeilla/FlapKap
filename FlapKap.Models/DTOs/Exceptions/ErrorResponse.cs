using FlapKap.Models.Response;

namespace FlapKap.Models.DTOs.Exceptions
{
    public class ErrorResponse
    {
        public ResponseStatus State { get; set; }
        public string Message { get; set; }

    }
}
