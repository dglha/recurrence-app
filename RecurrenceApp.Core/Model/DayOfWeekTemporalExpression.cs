using RecurrenceApp.Core.Enum;
using RecurrenceApp.Core.Utils;

namespace RecurrenceApp.Core.Model;

public record DayOfWeekTemporalExpression(Weekdays Weekdays) : TemporalExpression
{
    protected override bool Includes(DateOnly date)
    {
        return (Weekdays & TemporalExpressionUtil.ToFlag(date.DayOfWeek)) != 0;
    }
    
    public override IEnumerable<string> Errors() =>
        ErrorIf(Weekdays == Weekdays.None, "dayOfWeek needs at least one weekday.");
}