namespace SpaceXProject.api.Shared.Base.ResultPattern.ResultFactory.Interface;

public interface IResultFactory
{
    Result Success();
    Result<TValue> Success<TValue>(TValue value);
    Result Failure(Error.Error error);
    Result Failure(Error.Error error, ResultStatusEnum status);
    Result<TValue> Failure<TValue>(Error.Error error);
    Result<TValue> Failure<TValue>(Error.Error error, ResultStatusEnum status);
    Result FromStatus(ResultStatusEnum status);
    Result<TValue> FromStatus<TValue>(ResultStatusEnum status);
    Result Exception(Exception exception, string message, Error.Error? error = null);
    Result<TValue> Exception<TValue>(Exception exception, string message, Error.Error? error = null);
}

