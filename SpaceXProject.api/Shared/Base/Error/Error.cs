using System.Net;

namespace SpaceXProject.api.Shared.Base.Error;

public class Error
{
    public string Code { get; }
    public Exception? Exception { get; }
    public string[] Messages { get; }
    public HttpStatusCode? HttpStatusCode { get; }

    public static readonly Error None = new("None", ["No error."]);
    public static Error NullValue = new("error_null_value", ["Error, null value"]);
    public Error(string code, string[] messages, Exception? exception = null, HttpStatusCode? httpStatusCode = null)
    {
        Code = code;
        Messages = messages;
        Exception = exception;
        HttpStatusCode = httpStatusCode;
    }
}
