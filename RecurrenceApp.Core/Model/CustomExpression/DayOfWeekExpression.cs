using RecurrenceApp.Core.Enum;
using RecurrenceApp.Core.Utils;

namespace RecurrenceApp.Core.Model.CustomExpression;

/// <summary>
/// Eg.: Every Monday and Wednesday
/// </summary>
/// <param name="Weekdays"></param>
public record DayOfWeekTemporalExpression(Weekdays Weekdays) : TemporalExpression
{
    public override bool Includes(DateOnly date)
    {
        return (Weekdays & TemporalExpressionUtil.ToFlag(date.DayOfWeek)) != 0;
    }
    
    public override IEnumerable<string> Errors() =>
        ErrorIf(Weekdays == Weekdays.None, "dayOfWeek needs at least one weekday.");
}