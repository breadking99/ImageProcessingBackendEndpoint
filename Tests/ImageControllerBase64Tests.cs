using Api.Controllers;
using Api.Enums;
using Microsoft.AspNetCore.Mvc;
using Tests.Helpers;
using Xunit.Sdk;

namespace Tests;

public class ImageControllerBase64Tests
{
    [Theory]
    [InlineData("small-png", EEncodingType.PNG, false)]
    [InlineData("small-png", EEncodingType.PNG, true)]
    [InlineData("small-jpg", EEncodingType.JPG, false)]
    [InlineData("small-jpg", EEncodingType.JPG, true)]
    public void Post_Processing_ReturnsImage_ForBase64Samples(string sampleKey, EEncodingType encoding, bool blur)
    {
        if (!SampleBase64Data.TryGet(sampleKey, out var base64))
        {
            throw SkipException.ForSkip($"Missing base64 sample: {sampleKey}");
        }

        var controller = new ImageController();

        var result = controller.Post_Processing(base64, encoding, blur);

        var fileResult = Assert.IsType<FileContentResult>(result.Result);
        Assert.NotEmpty(fileResult.FileContents);
        Assert.Equal(encoding == EEncodingType.PNG ? "image/png" : "image/jpeg", fileResult.ContentType);
    }

    [Fact]
    public void Post_Processing_ReturnsBadRequest_ForEmptyBase64()
    {
        var controller = new ImageController();

        var result = controller.Post_Processing(string.Empty, EEncodingType.PNG, false);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}