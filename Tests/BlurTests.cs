using Api.Controllers;
using Api.Enums;
using Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tests.Helpers;
using Xunit.Sdk;

namespace Tests;

public class BlurTests
{
    [Theory]
    [InlineData("apple.png", EEncodingType.PNG, 12, 0.05)]
    [InlineData("apple.jpg", EEncodingType.JPG, 12, 0.05)]
    public async Task SampleFiles_Blurred(
        string inputFile,
        EEncodingType encoding,
        int maxPerChannelDifference,
        double maxDifferentPixelRatio)
    {
        if (!SampleImageData.TryGetSampleBytes(inputFile, out var inputBytes))
        {
            throw SkipException.ForSkip($"Missing input sample file: {inputFile}");
        }

        var expectedFile = SampleImageData.GetBlurredFileName(inputFile);
        if (!SampleImageData.TryGetSampleBytes(expectedFile, out var expectedBytes))
        {
            throw SkipException.ForSkip($"Missing expected blurred file: {expectedFile}");
        }

        using var stream = new MemoryStream(inputBytes, writable: false);
        var file = new FormFile(stream, 0, inputBytes.Length, "file", inputFile)
        {
            Headers = new HeaderDictionary(),
            ContentType = SampleImageData.GetContentType(inputFile)
        };

        var controller = new ImageController();

        var result = await controller.Post_Converting(file, encoding, blur: true, cancellationToken: CancellationToken.None);

        var fileResult = Assert.IsType<FileContentResult>(result.Result);
        var options = new ImageComparisonOptions(maxPerChannelDifference, maxDifferentPixelRatio);
        ImageAssert.Similar(expectedBytes, fileResult.FileContents, options);
        Assert.Equal(encoding.ToContentType(), fileResult.ContentType);
    }
}
