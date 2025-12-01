using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace SpaceXProject.api.Shared.Helpers;
public static class JsonPropertyHelper
{
    private static readonly ConcurrentDictionary<MemberInfo, string> MemberCache = new();

    private static readonly ConcurrentDictionary<Type, string[]> TypeCache = new();

    public static string GetJsonPropertyName<T>(Expression<Func<T, object?>> expression)
    {
        var memberInfo = GetMemberInfo(expression);
        return MemberCache.GetOrAdd(memberInfo, ResolveJsonName);
    }

    public static string[] GetPropertyNames<T>()
    {
        return TypeCache.GetOrAdd(typeof(T), type =>
        {
            return type.GetProperties()
                .Select(ResolveJsonName)
                .ToArray();
        });
    }

    private static MemberInfo GetMemberInfo<T>(Expression<Func<T, object?>> expression)
    {
        return expression.Body switch
        {
            MemberExpression memberExpression => memberExpression.Member,
            UnaryExpression { Operand: MemberExpression operand } => operand.Member,
            _ => throw new ArgumentException("Expression must be a member access ", nameof(expression))
        };
    }

    private static string ResolveJsonName(MemberInfo memberInfo)
    {
        var attr = memberInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
        if (attr != null)
        {
            return attr.Name;
        }

        return char.ToLowerInvariant(memberInfo.Name[0]) + memberInfo.Name.Substring(1);
    }

}
