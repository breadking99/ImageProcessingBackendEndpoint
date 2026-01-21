using Api.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    [HttpPost("processing")]
    public IActionResult Post_Processing(IFormFile file, EEncodingType encoding) => Ok();
}