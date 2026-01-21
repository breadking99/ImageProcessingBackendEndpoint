using Api.Enums;
using Api.Native;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    [HttpPost("processing")]
    public IActionResult Post_Processing(IFormFile file, EEncodingType encoding)
    {
        bool isAcceptableType = false;

        foreach (EEncodingType type in Enum.GetValues<EEncodingType>())
        {
            bool isMatch = type
                .PossibleContentTypes()
                .Any(x => x.Equals(file.ContentType, StringComparison.OrdinalIgnoreCase));

            if (isMatch) isAcceptableType = true;
        }

        if (!isAcceptableType) return BadRequest("Unsupported encoding type.");

        Stream stream = file.OpenReadStream();
        stream = DllNative.ProcessImage(stream, encoding);
        string contenType = encoding
            .PossibleContentTypes()
            .FirstOrDefault() ?? "image/png";

        return File(stream, contenType);
    }

    [HttpGet("test/multiple-by-two")]
    public IActionResult Get_Test_MultipleByTwo([FromQuery] int value)
    {
		var result = DllNative.MultipleByTwo(value);

		return Ok(new { input = value, output = result });
    }
}