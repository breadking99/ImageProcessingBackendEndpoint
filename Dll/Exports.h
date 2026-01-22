#pragma once

#include "Types.h"

// Exported C ABI used by the C# P/Invoke layer.
#define EXPORTED_METHOD extern "C" __declspec(dllexport)

// Returns value * 2 (simple smoke-test export).
EXPORTED_METHOD int __cdecl MultipleByTwo(int value);

// Frees a buffer allocated by this DLL.
EXPORTED_METHOD void __cdecl FreeBuffer(void* p);

// Decodes, optionally blurs, and re-encodes image bytes.
EXPORTED_METHOD unsigned char* __cdecl ProcessImage(
    const unsigned char* input,
    int inputLength,
    EEncodingType encoding,
    bool blur,
    int* outputLength);