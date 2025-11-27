namespace SpaceXProject.api.Shared.Base.ResultPattern;

public enum ResultStatusEnum : byte
{
        Success,

        Failure,
        NotFound,
        NoActionPerformed,

        ValidationFailed,
        Exception,
        InvalidRequest,

        #region UserAuthentication

        EmailAlreadyExists,
        Unauthorized,

        #endregion
    
}
