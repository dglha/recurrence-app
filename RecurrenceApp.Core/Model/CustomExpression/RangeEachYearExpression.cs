namespace RecurrenceApp.Core.Model.CustomExpression;

public sealed record RangeEachYearExpression(int StartMonth, int EndMonth, int StartDay = 0, int EndDay = 0) : TemporalExpression
{
    public override bool Includes(DateOnly date)
    {
        var encodedMonthDay = ToMonthDay(date.Month, date.Day);
        var start = ToMonthDay(StartMonth, StartDay == 0 ? 1 : StartDay );
        var end = ToMonthDay(EndMonth, EndDay == 0 ? 31 : EndDay);

        if (start <= end)
        {
            return encodedMonthDay >= start && encodedMonthDay <= end; // Normal range (e.g., April 1 to Oct 31)
        }

        return encodedMonthDay >= start || encodedMonthDay <= end;
    }

    public override IEnumerable<string> Errors()
    {
        if (StartMonth is < 1 or > 12 || EndMonth is < 1 or > 12) 
        {
            yield return "rangeEachYear months must be 1..12.";
        }
        
        if (StartDay is < 0 or > 31 || EndDay is < 0 or > 31)
        {
            yield return "rangeEachYear days must be 0..31 (0 = whole month).";
        }
    }

    private static int ToMonthDay(int month, int day) => month * 100 + day;
}