using System.Net;

namespace WeatherBackend.Common.Errors;

public class NotFoundError : Exception
{
    public HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;

    public NotFoundError(string message) : base(message)
    {
    }

    public NotFoundError(string message, Exception innerException) : base(message, innerException)
    {
    }
}
