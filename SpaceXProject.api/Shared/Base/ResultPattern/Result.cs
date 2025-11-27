namespace SpaceXProject.api.Shared.Base.ResultPattern;
public class Result
{
    public ResultStatusEnum Status { get; }
    public bool IsSuccess => Status == ResultStatusEnum.Success;

    public Error.Error? Error { get; }

    public Result()
    {

    }

    public Result(ResultStatusEnum status = ResultStatusEnum.NoActionPerformed, Error.Error? error = null)
    {
        Status = status;
        Error = error;
    }
}

public class Result<TValue> : Result
{
    internal Result(TValue? value, ResultStatusEnum status, Error.Error? error = null) : base(status, error)
    {
        Value = value;
    }

    public TValue? Value { get; }
}
