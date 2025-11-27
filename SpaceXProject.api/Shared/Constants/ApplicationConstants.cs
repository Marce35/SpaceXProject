namespace SpaceXProject.api.Shared.Constants;

public static class ApplicationConstants
{
    public const int MaxIdentityFieldStringLength = 256;

    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 100;

    #region regexes

    public const string PasswordContainsNumericAndSpecialCharRegex = @"^(?=.*\d)(?=.*[\W_]).*$";

    #endregion
}
