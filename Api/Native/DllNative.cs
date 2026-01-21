using Api.Enums;
using System.Runtime.InteropServices;

namespace Api.Native;

public static class DllNative
{
    const string DllPath = @"Dll.dll";

    [DllImport(DllPath)]
    public static extern int MultipleByTwo(int value);

    public static Stream ProcessImage(Stream imageStream, EEncodingType encoding)
    {
        // Placeholder for actual native image processing logic
        return imageStream;
    }
}