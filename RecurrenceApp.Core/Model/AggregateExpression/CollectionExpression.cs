namespace RecurrenceApp.Core.Model.AggregateExpression;

public abstract record CollectionExpression(IReadOnlyList<TemporalExpression> Elements) : TemporalExpression
{
    /// <summary>Immutable rebuild: returns a new collection with the given elements (used by Resolve).</summary>
    protected abstract CollectionExpression With(IReadOnlyList<TemporalExpression> elements);

    public override IEnumerable<string> Errors() =>
        Elements.Count == 0
            ? ["A union/intersection needs at least one element."]
            : Elements.SelectMany(e => e.Errors());

    public override IEnumerable<string> CalendarRefs() => Elements.SelectMany(e => e.CalendarRefs());

    public override TemporalExpression Resolve(IReadOnlyDictionary<string, IReadOnlyCollection<DateOnly>> calendars) =>
        With(Elements.Select(e => e.Resolve(calendars)).ToList());
}