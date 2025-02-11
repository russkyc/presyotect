using System.Text.Json;

namespace Presyotect.Utilities;

public static class MapperExtensions
{
    public static T SimpleMap<T>(this object source)
    {
        var serialized = JsonSerializer.Serialize(source);
        var deserialized = JsonSerializer.Deserialize<T>(serialized)!;
        return deserialized;
    }
}