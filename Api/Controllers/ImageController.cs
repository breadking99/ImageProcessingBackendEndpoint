using Api.Enums;
using Api.Native;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Api.Controllers;

[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    [HttpPost]
    public ActionResult<byte[]> Post_Processing(
        IFormFile file,
        [FromQuery] EEncodingType encoding,
        [FromQuery] bool blur)
    {
        Stream stream = file.OpenReadStream();
        var bytes = new byte[stream.Length];
        stream.ReadExactly(bytes, 0, (int)stream.Length);
        var result = ProcessImage(bytes, encoding, blur);

        if (result.Result is not null) return result.Result;

        return File(result.Value!, encoding);
    }

    [HttpPost("processing")]
    public ActionResult<byte[]> Post_Processing(
        [FromQuery] string base64,
        [FromQuery] EEncodingType encoding,
        [FromQuery] bool blur)
    {
        if (string.IsNullOrWhiteSpace(base64)) return BadRequest("Base64 string is null or empty.");

        var bytes = Convert.FromBase64String(base64);
        var result = ProcessImage(bytes, encoding, blur);

        if (result.Result is not null) return result.Result;

        return File(result.Value!, encoding);
    }

    [HttpGet("test/multiple-by-two")]
    public IActionResult Get_Test_MultipleByTwo([FromQuery] int value)
    {
        var result = DllNative.MultipleByTwo(value);

        return Ok(new { input = value, output = result });
    }

    private FileContentResult File(byte[] bytes, EEncodingType encoding) =>
        File(bytes, encoding == EEncodingType.PNG ? "image/png" : "image/jpeg");

    private ObjectResult InternalServerError(string message) =>
        StatusCode(StatusCodes.Status500InternalServerError, message);

    private ActionResult<byte[]> ProcessImage(byte[] input, EEncodingType encoding, bool blur)
    {
        if (input is null || input.Length == 0) return BadRequest("Input bytes are empty.");

        IntPtr outputPtr = IntPtr.Zero;

        try
        {
            outputPtr = DllNative.ProcessImage(input, input.Length, encoding, blur, out var outputLength);

            if (outputPtr == IntPtr.Zero || outputLength <= 0) return InternalServerError("Native processing failed.");

            var output = new byte[outputLength];
            Marshal.Copy(outputPtr, output, 0, outputLength);

            return output;
        }
        catch (Exception ex)
        {
            return InternalServerError(ex.Message);
        }
        finally
        {
            if (outputPtr != IntPtr.Zero) DllNative.FreeBuffer(outputPtr);
        }
    }
}