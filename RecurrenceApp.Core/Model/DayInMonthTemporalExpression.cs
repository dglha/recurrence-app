namespace RecurrenceApp.Core.Model;

/// <summary>
/// "2nd Monday" is (Monday, 2); "last Friday" is (Friday, -1).
/// Positive counts run from the start of the month, negative counts from the end.
/// </summary>
/// <param name="Day"></param>
/// <param name="Count"></param>
public sealed record DayInMonthTemporalExpression(DayOfWeek Day, int Count) : TemporalExpression
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
}