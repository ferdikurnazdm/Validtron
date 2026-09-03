using System.Linq.Expressions;

namespace Validtron.Internal;

internal static class PropertyPathHelper
{
    public static string GetPropertyPath(LambdaExpression expression)
    {
        var body = expression.Body;

        if (body is UnaryExpression unaryExpression)
        {
            body = unaryExpression.Operand;
        }

        var members = new Stack<string>();

        while (body is MemberExpression memberExpression)
        {
            members.Push(memberExpression.Member.Name);

            body = memberExpression.Expression!;
        }

        return members.Count > 0
            ? string.Join(".", members)
            : body.ToString();
    }

    public static string CombineChildPropertyName(string parentPropertyName, string childPropertyName)
    {
        return string.IsNullOrEmpty(childPropertyName)
            ? parentPropertyName
            : $"{parentPropertyName}.{childPropertyName}";
    }
}
