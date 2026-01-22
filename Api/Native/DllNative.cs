using Api.Enums;
using System.Runtime.InteropServices;

namespace Api.Native;

public static class DllNative
{
    const string DllPath = @"Dll.dll";

    [DllImport(DllPath)]
    public static extern int MultipleByTwo(int value);

    [DllImport(DllPath)]
    public static extern void FreeBuffer(IntPtr p);

    [DllImport(DllPath)]
    public static extern IntPtr ProcessImage(
        byte[] input,
        int inputLength,
        EEncodingType encoding,
        [MarshalAs(UnmanagedType.I1)] bool blur,
        out int outputLength);
}
