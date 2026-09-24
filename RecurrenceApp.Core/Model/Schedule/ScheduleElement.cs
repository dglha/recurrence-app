namespace RecurrenceApp.Core.Model.Schedule;

public sealed record ScheduleElement(string Event, TemporalExpression When, DateOnly? From = null, DateOnly? Until = null)
{
    public bool IsOccurring(string eventName, DateOnly date) =>
        eventName == Event
        && (From is null || date >= From)
        && (Until is null || date <= Until)
        && When.Includes(date);

    /// <summary>Matching dates in [from, to], clamped to the element's own From/Until.</summary>
    public IEnumerable<DateOnly> Dates(DateOnly from, DateOnly to)
    {
        var lowerBound = From is { } fromBound && fromBound > from ? fromBound : from;
        var upperBound = Until is { } untilBound && untilBound < to ? untilBound : to;
        return When.Expand(lowerBound, upperBound);
    }
}