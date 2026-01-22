// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <cstddef>
#include <cstring>
#include <cstdlib>
#include <vector>
#include <opencv2/core.hpp>
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

static void ApplyGaussianBlur(const cv::Mat& input, cv::Mat& output)
{
    cv::setNumThreads(cv::getNumberOfCPUs());
    const cv::Size kernelSize(5, 5);
    const int radius = kernelSize.width / 2;

    if (input.cols <= kernelSize.width || input.rows <= kernelSize.height)
    {
        cv::GaussianBlur(input, output, kernelSize, 0.0, 0.0, cv::BORDER_DEFAULT);
        return;
    }

    output.create(input.size(), input.type());

    const int tileSize = 128;
    const int tilesX = (input.cols + tileSize - 1) / tileSize;
    const int tilesY = (input.rows + tileSize - 1) / tileSize;
    const int totalTiles = tilesX * tilesY;

    cv::parallel_for_(cv::Range(0, totalTiles), [&](const cv::Range& range)
    {
        for (int idx = range.start; idx < range.end; ++idx)
        {
            const int tileY = idx / tilesX;
            const int tileX = idx % tilesX;

            const int x = tileX * tileSize;
            const int y = tileY * tileSize;
            const int w = std::min(tileSize, input.cols - x);
            const int h = std::min(tileSize, input.rows - y);

            const cv::Rect tileRect(x, y, w, h);
            const cv::Rect expandedRect(
                std::max(0, x - radius),
                std::max(0, y - radius),
                std::min(input.cols - std::max(0, x - radius), w + 2 * radius),
                std::min(input.rows - std::max(0, y - radius), h + 2 * radius));

            const cv::Mat tileInput = input(expandedRect);
            cv::Mat tileBlurred;
            cv::GaussianBlur(tileInput, tileBlurred, kernelSize, 0.0, 0.0, cv::BORDER_DEFAULT);

            const int offsetX = x - expandedRect.x;
            const int offsetY = y - expandedRect.y;
            const cv::Rect copyRect(offsetX, offsetY, w, h);
            tileBlurred(copyRect).copyTo(output(tileRect));
        }
    });
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
        cv::Mat blurred;
        ApplyGaussianBlur(image, blurred);
        image = blurred;
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
