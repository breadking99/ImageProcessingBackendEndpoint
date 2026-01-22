// Exports.cpp : C ABI exports used by the managed layer.
#include "pch.h"
#include "Exports.h"
#include "ImageProcessing.h"
#include <cstdlib>
#include <cstring>
#include <vector>

namespace
{
    // Allocates a native buffer and copies the encoded image bytes into it.
    unsigned char* AllocateAndCopy(const std::vector<unsigned char>& data, int* outputLength)
    {
        if (!outputLength)
        {
            return nullptr;
        }

        if (data.empty())
        {
            *outputLength = 0;
            return nullptr;
        }

        auto* output = static_cast<unsigned char*>(std::malloc(data.size()));
        if (!output)
        {
            *outputLength = 0;
            return nullptr;
        }

        std::memcpy(output, data.data(), data.size());
        *outputLength = static_cast<int>(data.size());
        return output;
    }
}

// Releases memory allocated by AllocateAndCopy.
EXPORTED_METHOD void __cdecl FreeBuffer(void* p)
{
    std::free(p);
}

// Decodes input bytes, optionally blurs, and returns encoded output bytes.
EXPORTED_METHOD unsigned char* __cdecl ProcessImage(
    const unsigned char* input,
    int inputLength,
    EEncodingType encoding,
    bool blur,
    int* outputLength)
{
    // Validate the mandatory input and output pointers.
    if (!input || inputLength <= 0 || !outputLength)
    {
        if (outputLength)
        {
            *outputLength = 0;
        }
        return nullptr;
    }

    // Run the OpenCV pipeline and return the encoded bytes.
    std::vector<unsigned char> encoded;
    if (!ImageProcessor::TryProcessImage(input, inputLength, encoding, blur, encoded))
    {
        *outputLength = 0;
        return nullptr;
    }

    // Marshal the output bytes to a native buffer for the caller.
    return AllocateAndCopy(encoded, outputLength);
}
