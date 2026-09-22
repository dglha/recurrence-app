using System.Text.Json;
using System.Text.Json.Serialization;

namespace RecurrenceApp.Core.Model;

/// <summary>
/// JSON form stored in recurring_series.expression, e.g.
/// {"type":"difference","included":{...},"excluded":{"type":"holidays","calendar":"US-MA"}}.
/// PostgreSQL jsonb does not preserve key order, so "type" can come back anywhere in the object;
/// AllowOutOfOrderMetadataProperties (System.Text.Json 9+) makes the deserializer accept that.
/// </summary>
public static class TemporalJson
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
        AllowOutOfOrderMetadataProperties = true
    };

    public static string Serialize(TemporalExpression expression) =>
        JsonSerializer.Serialize(expression, Options); // static type = base, so the discriminator is emitted

    public static TemporalExpression Deserialize(string json) =>
        JsonSerializer.Deserialize<TemporalExpression>(json, Options)
        ?? throw new JsonException("Expression JSON is empty.");
}