using Api.Enums;
using Api.Native;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    [HttpPost("processing")]
    public IActionResult Post_Processing(string base64, EEncodingType encoding)
    {
        if (string.IsNullOrWhiteSpace(base64)) return BadRequest("Base64 string is null or empty.");

        var decodedBytes = Convert.FromBase64String(base64);

        return Ok();
    }

    [HttpGet("test/multiple-by-two")]
    public IActionResult Get_Test_MultipleByTwo([FromQuery] int value)
    {
		var result = DllNative.MultipleByTwo(value);

		return Ok(new { input = value, output = result });
    }

    private ObjectResult InternalServerError(string message) =>
        StatusCode(StatusCodes.Status500InternalServerError, message);
}