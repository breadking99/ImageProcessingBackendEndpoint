// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <cstddef>
#include <cstring>
#include <cstdlib>

#define EXPORTED_METHOD extern "C" __declspec(dllexport)

EXPORTED_METHOD int __cdecl MultipleByTwo(int value)
{
	return value * 2;
}

enum EEncodingType
{
    PNG,
    JPG
};

EXPORTED_METHOD unsigned char* __cdecl ProcessImage(
    const unsigned char* input,
    int inputLength,
    EEncodingType /*encoding*/,
    bool /*blur*/,
    int* outputLength)
{
    if (!input || inputLength <= 0 || !outputLength)
    {
        if (outputLength) { *outputLength = 0; }
        return nullptr;
    }

    auto* output = static_cast<unsigned char*>(std::malloc(static_cast<size_t>(inputLength)));
    if (!output)
    {
        *outputLength = 0;
        return nullptr;
    }

    std::memcpy(output, input, static_cast<size_t>(inputLength));
    *outputLength = inputLength;
    return output;
}

EXPORTED_METHOD void __cdecl FreeBuffer(void* p)
{
	std::free(p);
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

