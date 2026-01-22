using Api.Controllers;
using Api.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tests.Helpers;
using Xunit.Sdk;

namespace Tests;

public class ImageControllerFileTests
{
    [Theory]
    [InlineData("sample.png", EEncodingType.PNG, false)]
    [InlineData("sample.png", EEncodingType.PNG, true)]
    [InlineData("sample.jpg", EEncodingType.JPG, false)]
    [InlineData("sample.jpg", EEncodingType.JPG, true)]
    public void Post_Processing_ReturnsImage_ForSampleFiles(string fileName, EEncodingType encoding, bool blur)
    {
        if (!SampleImageData.TryGetSampleBytes(fileName, out var bytes))
        {
            throw SkipException.ForSkip($"Missing sample file: {fileName}");
        }

        using var stream = new MemoryStream(bytes, writable: false);
        IFormFile file = new FormFile(stream, 0, bytes.Length, "file", fileName);

        var controller = new ImageController();

        var result = controller.Post_Converting(file, encoding, blur);

        var fileResult = Assert.IsType<FileContentResult>(result.Result);
        Assert.NotEmpty(fileResult.FileContents);
        Assert.Equal(encoding == EEncodingType.PNG ? "image/png" : "image/jpeg", fileResult.ContentType);
    }
}