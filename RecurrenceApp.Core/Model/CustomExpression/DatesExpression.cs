namespace RecurrenceApp.Core.Model.CustomExpression;

public sealed record DatesExpression(IReadOnlyList<DateOnly> Values) : TemporalExpression
{
    private readonly HashSet<DateOnly> _valueSet = [..Values];

    public override bool Includes(DateOnly date)
    {
        return _valueSet.Contains(date);
    }
}