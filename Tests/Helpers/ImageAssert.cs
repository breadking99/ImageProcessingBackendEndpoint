using Api.Enums;
using Xunit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Tests.Helpers;

/// <summary>
/// Provides pixel-level comparison helpers for image tests.
/// </summary>
public static class ImageAssert
{
    /// <summary>
    /// Compares two images and asserts they are similar within the provided tolerances.
    /// </summary>
    public static void Similar(byte[] expected, byte[] actual, ImageComparisonOptions options)
    {
        using var expectedImage = Image.Load<Rgba32>(expected);
        using var actualImage = Image.Load<Rgba32>(actual);

        Assert.Equal(expectedImage.Width, actualImage.Width);
        Assert.Equal(expectedImage.Height, actualImage.Height);

        var totalPixels = (long)expectedImage.Width * expectedImage.Height;
        if (totalPixels == 0)
        {
            return;
        }

        var differentPixels = 0L;

        var expectedPixels = new Rgba32[totalPixels];
        var actualPixels = new Rgba32[totalPixels];
        expectedImage.CopyPixelDataTo(expectedPixels);
        actualImage.CopyPixelDataTo(actualPixels);

        for (var i = 0; i < expectedPixels.Length; i++)
        {
            if (ExceedsTolerance(expectedPixels[i], actualPixels[i], options.MaxPerChannelDifference))
            {
                differentPixels++;
            }
        }

        var ratio = differentPixels / (double)totalPixels;
        Assert.True(
            ratio <= options.MaxDifferentPixelRatio,
            $"Pixel difference ratio {ratio:P2} exceeds tolerance {options.MaxDifferentPixelRatio:P2}.");
    }

    /// <summary>
    /// Builds default tolerances based on the expected encoding.
    /// </summary>
    public static ImageComparisonOptions ForEncoding(EEncodingType encoding) =>
        encoding == EEncodingType.JPG
            ? new ImageComparisonOptions(MaxPerChannelDifference: 12, MaxDifferentPixelRatio: 0.05)
            : new ImageComparisonOptions(MaxPerChannelDifference: 0, MaxDifferentPixelRatio: 0.0);

    private static bool ExceedsTolerance(Rgba32 expected, Rgba32 actual, int maxPerChannelDifference)
    {
        return Math.Abs(expected.R - actual.R) > maxPerChannelDifference
            || Math.Abs(expected.G - actual.G) > maxPerChannelDifference
            || Math.Abs(expected.B - actual.B) > maxPerChannelDifference
            || Math.Abs(expected.A - actual.A) > maxPerChannelDifference;
    }
}

/// <summary>
/// Defines tolerances for pixel similarity checks.
/// </summary>
public readonly record struct ImageComparisonOptions(
    int MaxPerChannelDifference,
    double MaxDifferentPixelRatio);
