namespace Tasks.Api.Exceptions;

public class Ex(string message, string? reasonCode = null) : Exception(message)
{
    public string? ReasonCode { get; } = reasonCode;
}

public class NotAllowedEx(string message, string? reasonCode = null) : Ex(message, reasonCode) { }

public class ValidationEx(string message, string? reasonCode = null) : Ex(message, reasonCode) { }

public class NotFoundEx(string message, string? reasonCode = null) : Ex(message, reasonCode) { }



public static class ReasonCodes
{
    public static string InvalidTaskStateChange = "INVALID_TASK_STATE_CHANGE";
}