using Api.Controllers;
using Api.Enums;
using Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tests.Helpers;
using Xunit.Sdk;

namespace Tests;

public class ChangeEncodingTests
{
    [Theory]
    // Add a tolerance for lossy formats like JPG.
    // Example:
    // [InlineData("emoji1.png", "emoji1.jpg", EEncodingType.JPG, 12, 0.05)]
    [InlineData("apple.png", "apple.jpg", EEncodingType.JPG, 12, 0.05)]
    [InlineData("flower.png", "flower.jpg", EEncodingType.JPG, 12, 0.05)]
    [InlineData("apple.jpg", "apple.png", EEncodingType.PNG, 12, 0.05)]
    [InlineData("flower.jpg", "flower.png", EEncodingType.PNG, 12, 0.05)]
    public async Task SampleFiles_CanBeReencoded(
        string inputFile,
        string expectedFile,
        EEncodingType targetEncoding,
        int maxPerChannelDifference,
        double maxDifferentPixelRatio)
    {
        if (!SampleImageData.TryGetSampleBytes(inputFile, out var inputBytes))
        {
            throw SkipException.ForSkip($"Missing input sample file: {inputFile}");
        }

        if (!SampleImageData.TryGetSampleBytes(expectedFile, out var expectedBytes))
        {
            throw SkipException.ForSkip($"Missing expected sample file: {expectedFile}");
        }

        using var stream = new MemoryStream(inputBytes, writable: false);
        var file = new FormFile(stream, 0, inputBytes.Length, "file", inputFile)
        {
            Headers = new HeaderDictionary(),
            ContentType = SampleImageData.GetContentType(inputFile)
        };

        var controller = new ImageController();

        var result = await controller.Post_Converting(file, targetEncoding, blur: false, cancellationToken: CancellationToken.None);

        var fileResult = Assert.IsType<FileContentResult>(result.Result);
        var options = new ImageComparisonOptions(maxPerChannelDifference, maxDifferentPixelRatio);
        ImageAssert.Similar(expectedBytes, fileResult.FileContents, options);
        Assert.Equal(targetEncoding.ToContentType(), fileResult.ContentType);
    }
}
