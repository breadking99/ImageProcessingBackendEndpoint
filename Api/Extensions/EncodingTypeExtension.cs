using Api.Enums;

namespace Api.Extensions;

/// <summary>
/// Provides extension methods for the EEncodingType enumeration.
/// </summary>
public static class EncodingTypeExtension
{
    /// <summary>
    /// Converts the specified encoding type to its corresponding MIME content type string.
    /// </summary>
    /// <param name="encodingType">The encoding type to convert to a MIME content type.</param>
    /// <returns>A string representing the MIME content type for the specified encoding type. Returns "image/png" for PNG,
    /// "image/jpeg" for JPG, and "image/png" for any other value.</returns>
    public static string ToContentType(this EEncodingType encodingType) => encodingType switch
    {
        EEncodingType.PNG => "image/png",
        EEncodingType.JPG => "image/jpeg",
        _ => "image/png"
    };
}