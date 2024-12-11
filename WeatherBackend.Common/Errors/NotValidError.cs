using System.Net;

namespace WeatherBackend.Common.Errors;

public class NotValidError : Exception
{
    public HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;

    public NotValidError(string message) : base(message)
    {
    }

    public NotValidError(string message, Exception innerException) : base(message, innerException)
    {
    }
}