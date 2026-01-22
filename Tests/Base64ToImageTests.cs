using Api.Controllers;
using Api.Enums;
using Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Tests.Helpers;
using Xunit.Sdk;

namespace Tests;

public class Base64ToImageTests
{
    [Theory]
    // Add a tolerance for pixel comparisons if your expected images were produced by a different encoder.
    // Example:
    // [InlineData("emoji1.png", "emoji1.png", EEncodingType.PNG, 0, 0.0)]
    [InlineData("emoji1.png", "emoji1.png", EEncodingType.PNG, 0, 0.0)]
    [InlineData("emoji2.png", "emoji2.png", EEncodingType.PNG, 0, 0.0)]
    [InlineData("emoji3.png", "emoji3.png", EEncodingType.PNG, 0, 0.0)]
    public void Base64Samples_ReturnExpectedFiles(
        string base64Key,
        string expectedFile,
        EEncodingType encoding,
        int maxPerChannelDifference,
        double maxDifferentPixelRatio)
    {
        if (!SampleBase64Data.TryGet(base64Key, out var base64))
        {
            throw SkipException.ForSkip($"Missing base64 sample: {base64Key}");
        }

        if (!SampleImageData.TryGetSampleBytes(expectedFile, out var expectedBytes))
        {
            throw SkipException.ForSkip($"Missing expected sample file: {expectedFile}");
        }

        var controller = new ImageController();

        var result = controller.Post_Processing(base64, encoding, blur: false);

        var fileResult = Assert.IsType<FileContentResult>(result.Result);
        var options = new ImageComparisonOptions(maxPerChannelDifference, maxDifferentPixelRatio);
        ImageAssert.Similar(expectedBytes, fileResult.FileContents, options);
        Assert.Equal(encoding.ToContentType(), fileResult.ContentType);
    }
}
