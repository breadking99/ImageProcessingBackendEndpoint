using System.Runtime.InteropServices;

namespace Api.Native;

public static class DllNative
{
    const string DllPath = @"Dll.dll";

    [DllImport(DllPath)]
    public static extern int MultipleByTwo(int value);
}