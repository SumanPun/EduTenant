using System.Net;

namespace Application.Exceptions
{
    public class ForbiddenException(string message,
        List<string> errorMessage = default,
        HttpStatusCode statusCode = HttpStatusCode.Forbidden) : Exception(message)
    {
        public List<string> ErrorMessage { get; set; } = errorMessage;
        public HttpStatusCode StatusCode { get; set; } = statusCode;
    }
}
