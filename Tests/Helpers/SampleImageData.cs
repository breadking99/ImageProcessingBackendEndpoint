using System.Diagnostics.CodeAnalysis;

namespace Tests.Helpers;

/// <summary>
/// Loads sample image files from the Tests/Samples folder for tests.
/// </summary>
public static class SampleImageData
{
    /// <summary>
    /// Gets the absolute directory where sample files should be placed.
    /// </summary>
    public static string SamplesDirectory => Path.Combine(AppContext.BaseDirectory, "Samples");

    /// <summary>
    /// Returns the absolute path for a sample file name.
    /// </summary>
    public static string GetSamplePath(string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        return Path.Combine(SamplesDirectory, fileName);
    }

    /// <summary>
    /// Reads the file bytes for a sample image.
    /// </summary>
    public static byte[] GetSampleBytes(string fileName)
    {
        var path = GetSamplePath(fileName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Sample file not found: {path}");
        }

        // Read all bytes at once for simplicity in tests.
        return File.ReadAllBytes(path);
    }

    /// <summary>
    /// Reads the sample bytes and converts them to a base64 string.
    /// </summary>
    public static string GetSampleBase64(string fileName)
    {
        var bytes = GetSampleBytes(fileName);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Attempts to load the bytes without throwing if the file is missing.
    /// </summary>
    public static bool TryGetSampleBytes(
        string fileName,
        [NotNullWhen(true)] out byte[]? bytes)
    {
        bytes = null;
        var path = GetSamplePath(fileName);
        if (!File.Exists(path))
        {
            return false;
        }

        bytes = File.ReadAllBytes(path);
        return true;
    }
}