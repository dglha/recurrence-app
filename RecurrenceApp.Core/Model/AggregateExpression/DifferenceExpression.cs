namespace RecurrenceApp.Core.Model.AggregateExpression;

public record DifferenceExpression(TemporalExpression Included, TemporalExpression Excluded) : TemporalExpression
{
    public override bool Includes(DateOnly d) => Included.Includes(d) && !Excluded.Includes(d);

    public override IEnumerable<string> Errors() => Included.Errors().Concat(Excluded.Errors());

    public override IEnumerable<string> CalendarRefs() => Included.CalendarRefs().Concat(Excluded.CalendarRefs());

    public override TemporalExpression Resolve(IReadOnlyDictionary<string, IReadOnlyCollection<DateOnly>> calendars) =>
        new DifferenceExpression(Included.Resolve(calendars), Excluded.Resolve(calendars));
}