using Api.Enums;
using Api.Native;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    [HttpPost("processing")]
    public IActionResult Post_Processing(IFormFile file, EEncodingType encoding) => Ok();

    [HttpGet("test/multiple-by-two")]
    public IActionResult Get_Test_MultipleByTwo([FromQuery] int value)
    {
		var result = DllNative.MultipleByTwo(value);

		return Ok(new { input = value, output = result });
    }
}