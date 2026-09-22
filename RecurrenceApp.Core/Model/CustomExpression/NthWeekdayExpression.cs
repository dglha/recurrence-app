namespace RecurrenceApp.Core.Model.CustomExpression;

public sealed record NthWeekdayExpression(DayOfWeek Day, int Count) : TemporalExpression
{
    protected override bool Includes(DateOnly date)
    {
        if (date.DayOfWeek != Day)
        {
            return false;
        }
        
        var isForwardCount = Count > 0;

        var weekInMonth = isForwardCount
            ? (date.Day - 1) / 7 + 1
            : (DateTime.DaysInMonth(date.Year, date.Month) - date.Day) / 7 + 1;

        return weekInMonth == Math.Abs(Count);
    }

    public override IEnumerable<string> Errors()
    {
        return ErrorIf(Count == 0 || Math.Abs(Count) > 5, "dayInMonth.count must be 1..5 or -1..-5.");
    }
}