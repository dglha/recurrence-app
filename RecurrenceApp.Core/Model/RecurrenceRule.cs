using System.Numerics;
using RecurrenceApp.Core.Enum;
using RecurrenceApp.Core.Model.AggregateExpression;
using RecurrenceApp.Core.Model.CustomExpression;

namespace RecurrenceApp.Core.Model;

public sealed record RecurrenceRule(
    Freq Frequency,
    int Interval = 1,
    Weekdays Weekdays = Weekdays.None,
    int? MonthDay = null,
    int? SetPos = null
    )
{
    public static RecurrenceRule Daily(int every = 1) => new(Freq.Daily, every);
    
    public static RecurrenceRule Weekly(Weekdays weekdays, int every = 1) => new(Freq.Weekly, every, weekdays);
    
    public static RecurrenceRule MonthlyOnDay(int day, int every = 1) => new(Freq.Monthly, every, MonthDay: day);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="day"></param>
    /// <param name="setPos">1..4 for first..fourth, -1 for last.</param>
    /// <param name="every"></param>
    /// <returns></returns>
    public static RecurrenceRule MonthlyOnNth(Weekdays day, int setPos, int every = 1)  => new(Freq.Monthly, every, day, SetPos: setPos);

    #region Helper methods

    /// <summary>
    /// Null when valid; otherwise a human-readable reason. Mirrors the DB CHECK constraints.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public string? Validate()
    {
        if (Interval < 1)
        {
            return "Interval must be at least 1";
        }

        switch (Frequency)
        {
            case Freq.Weekly when Weekdays == Weekdays.None:
                return "A weekly rule needs at least one weekday";
            
            case Freq.Monthly when MonthDay is null == SetPos is null:
                return "A monthly rule needs exactly one of MonthDay or SetPos.";
            
            case Freq.Monthly when MonthDay is < 1 or > 31:
                return "MonthDay must be between 1 and 31.";
            
            case Freq.Monthly when SetPos is { } sp:
                if (sp is not (1 or 2 or 3 or 4 or -1))
                {
                    return "SetPos must be 1..4 or -1 (last).";
                }
                
                if (BitOperations.PopCount((uint)Weekdays) != 1)
                {
                    return "Nth-weekday rules need exactly one weekday.";
                }
                break;
            
            case Freq.Daily:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }

    public TemporalExpression ToExpression(DateOnly anchor) => Frequency switch
    {
        Freq.Daily => new IntervalExpression(PeriodUnit.Day, Interval, anchor),
        Freq.Weekly => WithInterval(PeriodUnit.Week, new DayOfWeekTemporalExpression(Weekdays), anchor),
        Freq.Monthly when MonthDay is { } monthDay => WithInterval(PeriodUnit.Month, new CustomExpression.DayOfMonthExpression(monthDay), anchor),
        Freq.Monthly => WithInterval(PeriodUnit.Month, new NthWeekdayExpression(ExtractDayOfWeek(Weekdays), SetPos!.Value), anchor),
        _ => throw new InvalidOperationException($"Unsupported frequency {Frequency}.")
    };
    
    private TemporalExpression WithInterval(PeriodUnit unit, TemporalExpression pattern, DateOnly anchor)
        => Interval == 1 ? pattern : new IntersectionExpression([new IntervalExpression(unit, Interval, anchor), pattern]);
    
    private static DayOfWeek ExtractDayOfWeek(Weekdays day) => (DayOfWeek)BitOperations.TrailingZeroCount((int)day);

    #endregion
}