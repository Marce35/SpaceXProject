namespace SpaceXProject.api.Shared.Constants;

public static class ApplicationConstants
{
    public const int MaxIdentityFieldStringLength = 256;

    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 100;

    #region Cookie settings

    public const int HttpCookieExpirationInMinutes = 60; // make sure it is always the same as the JWT expiration

    #endregion

    #region regexes

    public const string PasswordContainsNumericAndSpecialCharRegex = @"^(?=.*\d)(?=.*[\W_]).*$";

    #endregion
}
