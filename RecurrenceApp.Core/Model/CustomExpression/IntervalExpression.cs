using RecurrenceApp.Core.Enum;
using RecurrenceApp.Core.Utils;

namespace RecurrenceApp.Core.Model.CustomExpression;

public sealed record IntervalExpression(PeriodUnit Unit, int Every, DateOnly Anchor) : TemporalExpression
{
    public override bool Includes(DateOnly date)
    {
        return Unit switch
        {
            PeriodUnit.Day => (date.DayNumber - Anchor.DayNumber) % Every == 0,
            PeriodUnit.Week => (TemporalExpressionUtil.WeekStart(date).DayNumber - TemporalExpressionUtil.WeekStart(Anchor).DayNumber) / 7 % Every == 0,
            PeriodUnit.Month => ((date.Year - Anchor.Year) * 12 + date.Month - Anchor.Month) % Every == 0,
            _ => false
        };
    }

    public override IEnumerable<string> Errors()
    {
        return ErrorIf(Every < 1, "interval.Every must be greater than or equal to one.");
    }
}