namespace FlapKap.Models.Response
{
    public enum ResponseStatus
    {
        Error,
        Success,
        ValidationError,
        NotFound,
        BadRequest,
        UnAuthorized,
        Forbidden,
        InternalServerError,
        ActionNotAllowed,
        Created,
        InvalidInput,
        NotRegisteredUser,
        NotModified,
        UnprocessableEntity,
        NotImplemented
    }
}
