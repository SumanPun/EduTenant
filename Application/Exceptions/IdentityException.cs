using System.Net;

namespace Application.Exceptions
{
    public class IdentityException(
        string message,
        List<string> errorMessage = default,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
        : Exception(message)
    {
        public List<string> ErrorMessage { get; set; } = errorMessage;
        public HttpStatusCode StatusCode { get; set; } = statusCode;
    }
}
