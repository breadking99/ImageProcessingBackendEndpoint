using Api.Enums;
using System.Runtime.InteropServices;

#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time
#pragma warning disable CA1401 // P/Invokes should not be visible

namespace Api.Native;

/// <summary>
/// Provides managed declarations for native methods in the associated DLL, enabling interoperation with unmanaged code
/// for image processing and memory management tasks.
/// </summary>
/// <remarks>This class contains only static extern methods and is not intended to be instantiated. The methods
/// are used to invoke native functions from the underlying DLL, which may require appropriate native library deployment
/// and platform compatibility. Callers are responsible for managing any unmanaged resources, such as freeing buffers
/// allocated by native code.</remarks>
public static class DllNative
{
    #region CONSTANTS [DllPath]
    /// <summary>
    /// Specifies the file path to the external DLL used by the application.
    /// </summary>
    const string DllPath = @"Dll.dll";
    #endregion

    #region METHODS [MultipleByTwo, FreeBuffer, ProcessImage]
    /// <summary>
    /// Multiplies the specified integer value by two using the native library.
    /// </summary>
    /// <remarks>This method is implemented in unmanaged code. Ensure that the native library specified by
    /// DllPath is available at runtime.</remarks>
    /// <param name="value">The integer value to be multiplied by two.</param>
    /// <returns>The result of multiplying the specified value by two.</returns>
    [DllImport(DllPath)] public static extern int MultipleByTwo(int value);

    /// <summary>
    /// Releases the unmanaged memory buffer referenced by the specified pointer.
    /// </summary>
    /// <remarks>After calling this method, the memory referenced by the pointer is no longer valid and must
    /// not be accessed. This method should only be used to free buffers allocated by the corresponding native
    /// allocation function.</remarks>
    /// <param name="p">A pointer to the unmanaged memory buffer to be freed. This must be a valid pointer previously allocated by a
    /// compatible allocation method.</param>
    [DllImport(DllPath)] public static extern void FreeBuffer(IntPtr p);

    /// <summary>
    /// Processes the input image data using the specified encoding and optional blur effect.
    /// </summary>
    /// <remarks>This method is a platform invoke (P/Invoke) wrapper for a native image processing function.
    /// Ensure that the memory referenced by the returned pointer is properly released to avoid memory leaks.</remarks>
    /// <param name="input">The byte array containing the raw image data to process. Cannot be null.</param>
    /// <param name="inputLength">The number of bytes in the input array to process. Must be less than or equal to the length of <paramref
    /// name="input"/> and greater than zero.</param>
    /// <param name="encoding">The encoding type to use when processing the image.</param>
    /// <param name="blur">A value indicating whether to apply a blur effect to the processed image. Specify <see langword="true"/> to
    /// apply blur; otherwise, <see langword="false"/>.</param>
    /// <param name="outputLength">When this method returns, contains the number of bytes in the processed image data. This parameter is passed
    /// uninitialized.</param>
    /// <returns>A pointer to the processed image data. The caller is responsible for managing the memory referenced by the
    /// returned pointer.</returns>
    [DllImport(DllPath)] public static extern IntPtr ProcessImage(
        byte[] input,
        int inputLength,
        EEncodingType encoding,
        bool blur,
        out int outputLength);
    #endregion
}