using Api.Enums;
using Api.Extensions;
using Api.Native;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Api.Controllers;

/// <summary>
/// Provides API endpoints for image processing operations, including base64 processing and file conversion.
/// </summary>
/// <remarks>
/// This controller exposes endpoints for image manipulation tasks such as processing and converting images using
/// specified encoding types and optional blur effects. All endpoints are accessible under the 'image' route.
/// </remarks>
[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    #region POST [Processing, Converting, ToBase64]
    /// <summary>
    /// Processes an image provided as a base64-encoded string, applies optional blurring, and returns the result as a
    /// byte array in the specified encoding format.
    /// </summary>
    /// <param name="base64">The base64-encoded string representing the image to process. Cannot be null or empty.</param>
    /// <param name="encoding">The encoding format to use for the output image.</param>
    /// <param name="blur">A value indicating whether to apply a blur effect to the image. Set to <see langword="true"/> to blur the image;
    /// otherwise, <see langword="false"/>.</param>
    /// <param name="cancellationToken">A token used to cancel the request.</param>
    /// <returns>A byte array containing the processed image in the specified encoding format. Returns a bad request response if
    /// the input is invalid.</returns>
    [HttpPost("processing")]
    public async Task<ActionResult<byte[]>> Post_Processing(
        [FromBody] string base64,
        [FromQuery] EEncodingType encoding = EEncodingType.PNG,
        [FromQuery] bool blur = false,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(base64))
            return BadRequest("Base64 string is null or empty.");

        byte[] bytes;
        try
        {
            bytes = Convert.FromBase64String(base64);
        }
        catch (FormatException)
        {
            return BadRequest("Base64 string is invalid.");
        }

        try
        {
            var result = ProcessImage(bytes, encoding, blur, cancellationToken);

            if (result.Result is not null) return result.Result;

            return File(result.Value!, encoding);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(499, "Request was canceled.");
        }
    }

    /// <summary>
    /// Processes the uploaded image file and returns the converted image as a byte array, applying the specified
    /// encoding and optional blur effect.
    /// </summary>
    /// <param name="file">The image file to be processed. Must not be null and should contain valid image data.</param>
    /// <param name="encoding">The encoding type to apply to the output image.</param>
    /// <param name="blur">A value indicating whether to apply a blur effect to the image. Set to <see langword="true"/> to enable
    /// blurring; otherwise, <see langword="false"/>.</param>
    /// <param name="cancellationToken">A token used to cancel the request.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing a byte array of the processed image data in the specified encoding.
    /// Returns a file result if processing is successful. Returns a bad request response if the upload is missing, empty,
    /// or not a supported image type.</returns>
    [HttpPost("converting")]
    public async Task<ActionResult<byte[]>> Post_Converting(
        IFormFile file,
        [FromQuery] EEncodingType encoding = EEncodingType.PNG,
        [FromQuery] bool blur = false,
        CancellationToken cancellationToken = default)
    {
        if (!TryValidateImageUpload(file, out var validationError)) return BadRequest(validationError);

        await using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);

        var result = ProcessImage(memoryStream.ToArray(), encoding, blur, cancellationToken);

        if (result.Result is not null) return result.Result;

        return File(result.Value!, encoding);
    }

    /// <summary>
    /// Accepts an uploaded image file and returns its Base64 string representation.
    /// </summary>
    /// <param name="file">The image file to be converted to Base64. Must not be null and should contain valid image data.</param>
    /// <param name="cancellationToken">A token used to cancel the request.</param>
    /// <returns>A Base64 string representing the uploaded image, or a bad request response if the file is invalid.</returns>
    [HttpPost("to-base64")]
    public async Task<ActionResult<string>> Post_ToBase64(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (!TryValidateImageUpload(file, out var validationError))
            return BadRequest(validationError);

        await using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        var bytes = memoryStream.ToArray();
        var base64 = Convert.ToBase64String(bytes);

        return base64;
    }
    #endregion

    #region METHODS Private [File, InternalServerError, ProcessImage, TryValidateImageUpload]
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
    /// Processes the specified image data asynchronously, applying optional encoding and blur effects.
    /// </summary>
    /// <param name="input">The image data to process, represented as a byte array. Cannot be null.</param>
    /// <param name="encoding">The encoding type to apply to the processed image.</param>
    /// <param name="blur">A value indicating whether to apply a blur effect to the image. Set to <see langword="true"/> to enable
    /// blurring; otherwise, <see langword="false"/>.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ActionResult{T}"/>
    /// whose value is a byte array with the processed image data.</returns>
    private async Task<ActionResult<byte[]>> ProcessImageAsync(byte[] input, EEncodingType encoding, bool blur, CancellationToken cancellationToken) =>
        await Task.Run(() => ProcessImage(input, encoding, blur, cancellationToken), cancellationToken);

    /// <summary>
    /// Processes the input image data using the specified encoding and optional blur effect.
    /// </summary>
    /// <param name="input">The byte array containing the image data to process. Cannot be null or empty.</param>
    /// <param name="encoding">The encoding type to use when processing the image.</param>
    /// <param name="blur">A value indicating whether to apply a blur effect to the image. Set to <see langword="true"/> to enable
    /// blurring; otherwise, <see langword="false"/>.</param>
    /// <param name="cancellationToken">A token used to cancel the request.</param>
    /// <returns>An <see cref="ActionResult{T}"/> containing the processed image data as a byte array if successful; otherwise,
    /// an error result if processing fails or the input is invalid.</returns>
    private ActionResult<byte[]> ProcessImage(byte[] input, EEncodingType encoding, bool blur, CancellationToken cancellationToken)
    {
        if (input is null || input.Length == 0) return BadRequest("Input bytes are empty.");

        IntPtr outputPtr = IntPtr.Zero;

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            outputPtr = DllNative.ProcessImage(input, input.Length, encoding, blur, out var outputLength);

            if (outputPtr == IntPtr.Zero || outputLength <= 0) return InternalServerError("Native processing failed.");

            var output = new byte[outputLength];
            Marshal.Copy(outputPtr, output, 0, outputLength);

            return output;
        }
        catch (OperationCanceledException)
        {
            return StatusCode(StatusCodes.Status408RequestTimeout, "Request was canceled.");
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

    /// <summary>
    /// Validates that an uploaded file is a supported image type.
    /// </summary>
    /// <param name="file">The uploaded file to validate.</param>
    /// <param name="error">An error message describing why validation failed, if any.</param>
    /// <returns><see langword="true"/> if the file looks like a supported image; otherwise <see langword="false"/>.</returns>
    /// <remarks>
    /// This method validates the upload using both the declared MIME type (<see cref="IFormFile.ContentType"/>) and by
    /// inspecting the first few bytes of the payload ("magic bytes") to reduce reliance on client-provided metadata.
    /// Currently supports PNG and JPEG.
    /// </remarks>
    private static bool TryValidateImageUpload(IFormFile? file, out string error)
    {
        if (file is null)
        {
            error = "File is required.";

            return false;
        }

        if (file.Length <= 0)
        {
            error = "File is empty.";

            return false;
        }

        if (string.IsNullOrWhiteSpace(file.ContentType)
            || !file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            error = "Only image uploads are allowed.";

            return false;
        }

        try
        {
            using var stream = file.OpenReadStream();
            Span<byte> header = stackalloc byte[8];
            var read = stream.Read(header);

            if (read < 2)
            {
                error = "File does not contain valid image data.";
               
                return false;
            }

            var isPng = read >= 8
                && header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47
                && header[4] == 0x0D && header[5] == 0x0A && header[6] == 0x1A && header[7] == 0x0A;
            var isJpeg = header[0] == 0xFF && header[1] == 0xD8;

            if (!isPng && !isJpeg)
            {
                error = "Unsupported image format. Only PNG and JPEG are supported.";
                return false;
            }

            error = string.Empty;
            return true;
        }
        catch
        {
            error = "Unable to read uploaded file.";
            return false;
        }
    }
    #endregion
}
