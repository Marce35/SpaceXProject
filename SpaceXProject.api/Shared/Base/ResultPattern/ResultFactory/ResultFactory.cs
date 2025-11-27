using SpaceXProject.api.Shared.Base.ResultPattern.ResultFactory.Interface;
using SpaceXProject.api.Shared.Base.ResultPattern.Extensions;

namespace SpaceXProject.api.Shared.Base.ResultPattern.ResultFactory;

public class ResultFactory : IResultFactory
{
    private readonly ILogger<ResultFactory>? _logger;

    public ResultFactory()
    {

    }

    public ResultFactory(ILogger<ResultFactory>? logger)
    {
        _logger = logger;
    }

    public Result Success() => new(
        status: ResultStatusEnum.Success,
        error: null);

    public Result<TValue> Success<TValue>(TValue value) => new(
        value: value,
        status: ResultStatusEnum.Success,
        error: null);

    public Result Failure(Error.Error error) => new(
        status: ResultStatusEnum.Failure,
        error: error);

    public Result Failure(Error.Error error, ResultStatusEnum status) => new(
        error: error,
        status: status);

    public Result<TValue> Failure<TValue>(Error.Error error) => new(
        value: default,
        status: ResultStatusEnum.Failure,
        error: error);

    public Result<TValue> Failure<TValue>(Error.Error error, ResultStatusEnum status) => new(
        value: default,
        status: status,
        error: error);

    public Result FromStatus(ResultStatusEnum status) => new(
        status: status,
        error: status.ResolveError());

    public Result<TValue> FromStatus<TValue>(ResultStatusEnum status) => new(
        value: default,
        status: status,
        error: status.ResolveError());

    public Result Exception(Exception exception, string message, Error.Error? error = null)
    {
        _logger?.LogError(exception, "Exception: {Message}. Error: {Error}", message, error?.Messages ?? ["None"]);
        return new Result(
            status: ResultStatusEnum.Exception,
            error: error ?? new Error.Error(
                code: "Exception",
                messages: ["An exception occurred with message: " + message]));
    }

    public Result<TValue> Exception<TValue>(Exception exception, string message, Error.Error? error = null)
    {
        _logger?.LogError(exception, "Exception: {Message}. Error: {Error}", message, error?.Messages ?? ["None"]);
        return new Result<TValue>(
            value: default,
            status: ResultStatusEnum.Exception,
            error: error ?? new Error.Error(
                code: "Exception",
                messages: ["An exception occurred with message: " + message]));
    }
}

