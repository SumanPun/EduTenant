using System.Net;

namespace Application.Exceptions
{
    public class UnAuthorizedException(
        string message, 
        List<string> errorMessage = default, 
        HttpStatusCode statusCode = HttpStatusCode.Unauthorized) : Exception(message)
    {
        public List<string> ErrorMessage { get; set; } = errorMessage;
        public HttpStatusCode StatusCode { get; set; } = statusCode;
    }
}
