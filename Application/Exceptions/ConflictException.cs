using System.Net;

namespace Application.Exceptions
{
    public class ConflictException(
        string message,
        List<string> errorMessage = default,
        HttpStatusCode statusCode = HttpStatusCode.Conflict) : Exception(message)
    {
        public List<string> ErrorMessage { get; set; } = errorMessage;
        public HttpStatusCode StatusCode { get; set; } = statusCode;
    }
}
