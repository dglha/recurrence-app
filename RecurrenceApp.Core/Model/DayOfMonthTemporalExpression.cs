namespace RecurrenceApp.Core.Model;

/// <summary>
/// "On the 31st". Business decision: day 31 clamps to the last day of shorter months, so a rent reminder still fires.
/// </summary>
/// <param name="Day"></param>
public record DayOfMonthTemporalExpression(int Day) : TemporalExpression
{
    protected override bool Includes(DateOnly date)
    {
        return date.Day == Math.Min(Day, DateTime.DaysInMonth(date.Year, date.Month));
    }

    public override IEnumerable<string> Errors()
    {
        return ErrorIf(Day is < 1 or > 31, "Day must be between 1 and 31");
    }
}