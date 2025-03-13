using System.Net;

namespace Application.Exceptions
{
    public class NotFoundException(
        string message,
        List<string> errorMessage = default,
        HttpStatusCode statusCode = HttpStatusCode.NotFound) : Exception(message)
    {
        public List<string> ErrorMessage { get; set; } = errorMessage;
        public HttpStatusCode StatusCode { get; set; } = statusCode;
    }
}
