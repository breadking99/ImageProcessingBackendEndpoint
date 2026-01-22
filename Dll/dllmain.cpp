// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <cstddef>
#include <cstring>
#include <cstdlib>
#include <vector>
#include <opencv2/imgcodecs.hpp>
#include <opencv2/imgproc.hpp>

#define EXPORTED_METHOD extern "C" __declspec(dllexport)

enum EEncodingType
{
    PNG,
    JPG
};

EXPORTED_METHOD int __cdecl MultipleByTwo(int value)
{
	return value * 2;
}

EXPORTED_METHOD void __cdecl FreeBuffer(void* p)
{
    std::free(p);
}

static bool TryDecodeImage(const unsigned char* input, int inputLength, cv::Mat& output)
{
    if (!input || inputLength <= 0)
    {
        return false;
    }

    std::vector<unsigned char> buffer(input, input + inputLength);
    output = cv::imdecode(buffer, cv::IMREAD_UNCHANGED);
    return !output.empty();
}

static void ApplyGaussianBlur(cv::Mat& image)
{
    cv::setNumThreads(cv::getNumberOfCPUs());
    cv::GaussianBlur(image, image, cv::Size(5, 5), 0.0, 0.0, cv::BORDER_DEFAULT);
}

static bool TryEncodeImage(const cv::Mat& input, EEncodingType encoding, std::vector<unsigned char>& output)
{
    const char* extension = encoding == JPG ? ".jpg" : ".png";
    return cv::imencode(extension, input, output);
}

EXPORTED_METHOD unsigned char* __cdecl ProcessImage(
    const unsigned char* input,
    int inputLength,
    EEncodingType encoding,
    bool blur,
    int* outputLength)
{
    if (!input || inputLength <= 0 || !outputLength)
    {
        if (outputLength) { *outputLength = 0; }
        return nullptr;
    }

    cv::Mat image;
    if (!TryDecodeImage(input, inputLength, image))
    {
        *outputLength = 0;
        return nullptr;
    }

    if (blur)
    {
        ApplyGaussianBlur(image);
    }

    std::vector<unsigned char> encoded;
    if (!TryEncodeImage(image, encoding, encoded))
    {
        *outputLength = 0;
        return nullptr;
    }

    auto* output = static_cast<unsigned char*>(std::malloc(encoded.size()));
    if (!output)
    {
        *outputLength = 0;
        return nullptr;
    }

    std::memcpy(output, encoded.data(), encoded.size());
    *outputLength = static_cast<int>(encoded.size());
    return output;
}

static BOOL APIENTRY DllMain( HMODULE hModule,
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