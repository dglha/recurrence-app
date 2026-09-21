using RecurrenceApp.Core.Enum;

namespace RecurrenceApp.Core.Utils;

public static class TemporalExpressionUtil
{
    public static Weekdays ToFlag(DayOfWeek w) => (Weekdays)(1 << (int)w);

    /// <summary>
    /// Weeks are Monday-based so "every 2 weeks on Mon/Wed" is stable regardless of the start weekday.
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static DateOnly WeekStart(DateOnly d) => d.AddDays(-(((int)d.DayOfWeek + 6) % 7));
}