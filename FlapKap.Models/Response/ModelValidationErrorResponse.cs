namespace FlapKap.Models.Response
{
    public class ModelValidationErrorResponse
    {
        public ModelValidationErrorResponse()
        {
            Errors = [];
        }
        public ResponseStatus State { get; set; }
        public string Message { get; set; }
        public List<ErrorModel> Errors { get; set; }

    }
}
