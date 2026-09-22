namespace RecurrenceApp.Core.Model.CustomExpression;

public sealed record DayOfMonthExpression(int Day) : TemporalExpression
{
    protected override bool Includes(DateOnly d) => d.Day == Math.Min(Day, DateTime.DaysInMonth(d.Year, d.Month));

    public override IEnumerable<string> Errors()
    {
        return ErrorIf(Day is < 1 or > 31, "dayOfMonth.day must be 1..31.");
    }
}