using RecurrenceApp.Core.Model;

namespace RecurrenceApp.Core.Expression;

/// <summary>
/// Extension methods for TemporalExpression
/// </summary>
public static class TemporalExpressionExtensions
{
    /// <summary>
    /// Validates the expression and throws if any problems exist.
    /// Centralizes the repeated "Errors().FirstOrDefault() → throw" pattern.
    /// </summary>
    /// <param name="expr"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static TemporalExpression EnsureValid(this TemporalExpression expr)
    {
        if (expr.Errors().FirstOrDefault() is { } error)
        {
            throw new InvalidOperationException($"Invalid expression: {error}");
        }

        return expr;
    }
}