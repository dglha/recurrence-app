namespace RecurrenceApp.Core.Model.CustomExpression;

public sealed record HolidaysExpression(string Calendar) : TemporalExpression
{
    // Fail loudly: silently treating an unresolved calendar as "no holidays" would schedule on holidays.
    protected override bool Includes(DateOnly d) =>
        throw new InvalidOperationException($"Holiday calendar '{Calendar}' is not resolved; call Resolve() first.");

    public override IEnumerable<string> CalendarRefs() => [Calendar];

    public override TemporalExpression Resolve(IReadOnlyDictionary<string, IReadOnlyCollection<DateOnly>> calendars) =>
        calendars.TryGetValue(Calendar, out var days)
            ? new DatesExpression(days.ToList()) // An empty calendar is valid (no holidays) - only a missing name is an error.
            : throw new KeyNotFoundException($"Unknown holiday calendar '{Calendar}'.");
}