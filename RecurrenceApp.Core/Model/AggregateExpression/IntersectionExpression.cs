namespace RecurrenceApp.Core.Model.AggregateExpression;

public record IntersectionExpression(IReadOnlyList<TemporalExpression> Elements) : CollectionExpression(Elements)
{
    public override bool Includes(DateOnly date)
    {
        return Elements.All(x => x.Includes(date));
    }

    protected override CollectionExpression With(IReadOnlyList<TemporalExpression> elements)
    {
        return new IntersectionExpression(elements);
    }
}