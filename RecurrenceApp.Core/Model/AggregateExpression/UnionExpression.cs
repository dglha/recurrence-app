namespace RecurrenceApp.Core.Model.AggregateExpression;

public record UnionExpression(IReadOnlyList<TemporalExpression> Elements) : CollectionExpression(Elements)
{
    public override bool Includes(DateOnly date)
    {
        return Elements.Any(x => x.Includes(date));
    }

    protected override CollectionExpression With(IReadOnlyList<TemporalExpression> elements)
    {
        return new UnionExpression(elements);
    }
}