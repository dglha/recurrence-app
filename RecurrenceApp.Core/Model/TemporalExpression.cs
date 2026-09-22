namespace RecurrenceApp.Core.Model;
using Calendars = IReadOnlyDictionary<string, IReadOnlyCollection<DateOnly>>;

public abstract record TemporalExpression
{
    /// <summary>
    /// true when the date matches
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    protected abstract bool Includes(DateOnly date);
    
    /// <summary>
    /// Human-readable validation problems; empty when the expression is well-formed.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<string> Errors() => [];
    
    /// <summary>
    /// Names of holiday calendars this expression references (must be resolved before use).
    /// </summary>
    public virtual IEnumerable<string> CalendarRefs() => Array.Empty<string>();

    /// <summary>
    /// Replaces calendar references with concrete dates. Keeps Includes() pure (no I/O): the caller
    /// loads calendars once per batch, then resolves each series' expression.
    /// </summary>
    /// <param name="calendars"></param>
    /// <returns></returns>
    public virtual TemporalExpression Resolve(Calendars calendars) => this;
    
    /// <summary>
    /// All matching dates in [from, to]. Day-by-day scan: fine for rolling windows (~90 days).
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public IEnumerable<DateOnly> Expand(DateOnly from, DateOnly to)
    {
        for (var date = from; date <= to; date = date.AddDays(1))
            if (Includes(date)) yield return date;
    }

    protected static IEnumerable<string> ErrorIf(bool badCondition, string message) =>
        badCondition ? new[] { message } : [];
}