namespace Tests.Helpers;

/// <summary>
/// Stores base64-encoded samples for tests.
/// </summary>
public static class SampleBase64Data
{
    // Add your base64 samples here, keyed by a friendly name.
    private static readonly Dictionary<string, string> Samples = new(StringComparer.OrdinalIgnoreCase)
    {
        // Example:
        // ["small-png"] = "iVBORw0KGgoAAAANSUhEUgAA..."
    };

    /// <summary>
    /// Gets a base64 sample by key or throws with a friendly message.
    /// </summary>
    public static string Get(string key)
    {
        if (TryGet(key, out var value))
        {
            return value;
        }

        throw new KeyNotFoundException($"Base64 sample not found: {key}");
    }

    /// <summary>
    /// Attempts to get a base64 sample by key.
    /// </summary>
    public static bool TryGet(string key, out string value)
    {
        value = string.Empty;
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        if (Samples.TryGetValue(key, out var foundValue) && foundValue is not null)
        {
            value = foundValue;
            return true;
        }

        return false;
    }
}