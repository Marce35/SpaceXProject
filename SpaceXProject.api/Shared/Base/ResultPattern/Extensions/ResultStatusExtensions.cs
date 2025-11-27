using SpaceXProject.api.Shared.Base.ResultPattern.Attributes;
using System.Reflection;

namespace SpaceXProject.api.Shared.Base.ResultPattern.Extensions;

public static class ResultStatusExtensions
{
    private static T? GetAttribute<T>(this Enum value) where T : Attribute
    {
        MemberInfo? member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        return member?.GetCustomAttribute<T>();
    }

    public static bool IsSuccess(this ResultStatusEnum status)
    {
        ResultStatusCustomConnotationAttribute? attribute = status.GetAttribute<ResultStatusCustomConnotationAttribute>();
        return attribute?.IsPositive ?? false;
    }

    public static Error.Error? ResolveError(this ResultStatusEnum status)
    {
        return status.IsSuccess()
            ? null
            : new Error.Error(
                code: "Status",
                messages: [$"Method returned with status: {status}"]);
    }
}

