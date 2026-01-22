using Api.Enums;
using Api.Extensions;
using Api.Native;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Api.Controllers;

/// <summary>
/// Provides API endpoints for performing image processing operations, including mathematical tests, image processing
/// from base64 strings, and image conversion from uploaded files.
/// </summary>
/// <remarks>This controller exposes endpoints for image manipulation tasks such as processing and converting
/// images using specified encoding types and optional blur effects. All endpoints are accessible under the 'image'
/// route. The controller is intended for use in web applications that require server-side image processing
/// capabilities.</remarks>
[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    #region GET [MultipleByTwo]
    /// <summary>
    /// Handles HTTP GET requests to multiply the specified value by two and returns the result.
    /// </summary>
    /// <param name="value">The integer value to be multiplied by two.</param>
    /// <returns>An <see cref="OkObjectResult"/> containing an object with the input value and the computed output.</returns>
    [HttpGet("test/multiple-by-two")]
    public IActionResult Get_Test_MultipleByTwo([FromQuery] int value)
    {
        var result = DllNative.MultipleByTwo(value);

        return Ok(new { input = value, output = result });
    }
    #endregion

    #region POST [Processing, Converting]
    /// <summary>
    /// Processes an image provided as a base64-encoded string, applies optional blurring, and returns the result as a
    /// byte array in the specified encoding format.
    /// </summary>
    /// <param name="base64">The base64-encoded string representing the image to process. Cannot be null or empty.</param>
    /// <param name="encoding">The encoding format to use for the output image.</param>
    /// <param name="blur">A value indicating whether to apply a blur effect to the image. Set to <see langword="true"/> to blur the image;
    /// otherwise, <see langword="false"/>.</param>
    /// <returns>A byte array containing the processed image in the specified encoding format. Returns a bad request response if
    /// the input is invalid.</returns>
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

    /// <summary>
    /// Processes the uploaded image file and returns the converted image as a byte array, applying the specified
    /// encoding and optional blur effect.
    /// </summary>
    /// <param name="file">The image file to be processed. Must not be null and should contain valid image data.</param>
    /// <param name="encoding">The encoding type to apply to the output image.</param>
    /// <param name="blur">A value indicating whether to apply a blur effect to the image. Set to <see langword="true"/> to enable
    /// blurring; otherwise, <see langword="false"/>.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a byte array of the processed image data in the specified encoding.
    /// Returns a file result if processing is successful, or the result from the image processing operation if
    /// available.</returns>
    [HttpPost("converting")]
    public ActionResult<byte[]> Post_Converting(
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
    #endregion

    #region METHODS Private [File, InternalServerError, ProcessImage]
    /// <summary>
    /// Creates a file result that returns the specified byte array as a file to the client, using the provided encoding
    /// type to determine the content type.
    /// </summary>
    /// <param name="bytes">The byte array containing the file data to send to the client. Cannot be null.</param>
    /// <param name="encoding">The encoding type that determines the MIME content type of the file.</param>
    /// <returns>A <see cref="FileContentResult"/> that, when executed, will prompt the client to download the file with the
    /// specified content type.</returns>
    private FileContentResult File(byte[] bytes, EEncodingType encoding) =>
        File(bytes, encoding.ToContentType());

    /// <summary>
    /// Creates an ObjectResult that produces a 500 Internal Server Error response with the specified error message.
    /// </summary>
    /// <param name="message">The error message to include in the response body. Cannot be null.</param>
    /// <returns>An ObjectResult with a 500 Internal Server Error status code containing the specified error message.</returns>
    private ObjectResult InternalServerError(string message) =>
        StatusCode(StatusCodes.Status500InternalServerError, message);

    /// <summary>
    /// Processes the input image data using the specified encoding and optional blur effect.
    /// </summary>
    /// <param name="input">The byte array containing the image data to process. Cannot be null or empty.</param>
    /// <param name="encoding">The encoding type to use when processing the image.</param>
    /// <param name="blur">A value indicating whether to apply a blur effect to the image. Set to <see langword="true"/> to enable
    /// blurring; otherwise, <see langword="false"/>.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing the processed image data as a byte array if successful; otherwise,
    /// an error result if processing fails or the input is invalid.</returns>
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
    #endregion
}