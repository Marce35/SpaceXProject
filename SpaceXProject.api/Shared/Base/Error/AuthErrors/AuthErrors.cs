namespace SpaceXProject.api.Shared.Base.Error.AuthErrors;

public static class AuthErrors
{
    public static Error DuplicateEmailError => new(
        code: "Auth_DuplicateEmail",
        messages: ["The email address is already in use."]
    );

    public static Error RegistrationFailedError(string[] details) => new(
        code: "Auth_RegistrationFailed",
        messages: details.Length > 0 ? details : ["User registration failed for an unknown reason."]
    );

    public static Error UnAuthorizedError => new(
        code: "Auth_UnAuthorized",
        messages: ["Invalid email or password."]
    );

    public static Error AccountLockedError => new(
        code: "Auth_AccountLocked",
        messages: ["The account is locked due to multiple failed login attempts."]
    );
}
