// ImageProcessing.cpp : OpenCV image pipeline implementation.
#include "pch.h"
#include "ImageProcessing.h"
#include <opencv2/imgcodecs.hpp>
#include <opencv2/imgproc.hpp>
#include <algorithm>

namespace
{
    constexpr int kKernelSize = 5;
    constexpr int kTileSize = 128;

    struct TileGrid
    {
        int tileSize{};
        int tilesX{};
        int tilesY{};
        int totalTiles{};
    };

    // Calculates the tile grid dimensions for a given image size.
    TileGrid BuildTileGrid(const cv::Mat& input, int tileSize)
    {
        TileGrid grid{};
        grid.tileSize = tileSize;
        grid.tilesX = (input.cols + tileSize - 1) / tileSize;
        grid.tilesY = (input.rows + tileSize - 1) / tileSize;
        grid.totalTiles = grid.tilesX * grid.tilesY;
        return grid;
    }

    // Returns the tile rectangle for a given tile index.
    cv::Rect BuildTileRect(const TileGrid& grid, int tileIndex, const cv::Size& imageSize)
    {
        const int tileY = tileIndex / grid.tilesX;
        const int tileX = tileIndex % grid.tilesX;

        const int x = tileX * grid.tileSize;
        const int y = tileY * grid.tileSize;
        const int w = std::min(grid.tileSize, imageSize.width - x);
        const int h = std::min(grid.tileSize, imageSize.height - y);

        return cv::Rect(x, y, w, h);
    }

    // Expands the tile rectangle to include the blur kernel radius.
    cv::Rect ExpandRect(const cv::Rect& rect, int radius, const cv::Size& imageSize)
    {
        const int expandedX = std::max(0, rect.x - radius);
        const int expandedY = std::max(0, rect.y - radius);
        const int expandedW = std::min(imageSize.width - expandedX, rect.width + 2 * radius);
        const int expandedH = std::min(imageSize.height - expandedY, rect.height + 2 * radius);
        return cv::Rect(expandedX, expandedY, expandedW, expandedH);
    }

    // Blurs a single tile with padding and copies the valid region to the output.
    void BlurTile(
        const cv::Mat& input,
        cv::Mat& output,
        const cv::Rect& tileRect,
        int radius,
        const cv::Size& kernelSize)
    {
        const cv::Rect expandedRect = ExpandRect(tileRect, radius, input.size());
        const cv::Mat tileInput = input(expandedRect);

        cv::Mat tileBlurred;
        cv::GaussianBlur(tileInput, tileBlurred, kernelSize, 0.0, 0.0, cv::BORDER_DEFAULT);

        const int offsetX = tileRect.x - expandedRect.x;
        const int offsetY = tileRect.y - expandedRect.y;
        const cv::Rect copyRect(offsetX, offsetY, tileRect.width, tileRect.height);
        tileBlurred(copyRect).copyTo(output(tileRect));
    }

    // Executes tile processing for a thread range.
    void ProcessTileRange(
        const cv::Range& range,
        const cv::Mat& input,
        cv::Mat& output,
        const TileGrid& grid,
        int radius,
        const cv::Size& kernelSize)
    {
        for (int idx = range.start; idx < range.end; ++idx)
        {
            const cv::Rect tileRect = BuildTileRect(grid, idx, input.size());
            BlurTile(input, output, tileRect, radius, kernelSize);
        }
    }
}

// Decodes an encoded image buffer into an OpenCV matrix.
bool TryDecodeImage(const unsigned char* input, int inputLength, cv::Mat& output)
{
    if (!input || inputLength <= 0)
    {
        return false;
    }

    // OpenCV expects a contiguous buffer for decoding.
    std::vector<unsigned char> buffer(input, input + inputLength);
    output = cv::imdecode(buffer, cv::IMREAD_UNCHANGED);
    return !output.empty();
}

// Applies a Gaussian blur using ROI tiling and parallel execution.
void ApplyGaussianBlur(const cv::Mat& input, cv::Mat& output)
{
    cv::setNumThreads(cv::getNumberOfCPUs());

    const cv::Size kernelSize(kKernelSize, kKernelSize);
    const int radius = kernelSize.width / 2;

    // For tiny images, a single blur pass is sufficient.
    if (input.cols <= kernelSize.width || input.rows <= kernelSize.height)
    {
        cv::GaussianBlur(input, output, kernelSize, 0.0, 0.0, cv::BORDER_DEFAULT);
        return;
    }

    output.create(input.size(), input.type());

    // Split into tiles so that parallel threads can work on ROIs.
    const TileGrid grid = BuildTileGrid(input, kTileSize);
    cv::parallel_for_(cv::Range(0, grid.totalTiles), [&](const cv::Range& range)
    {
        ProcessTileRange(range, input, output, grid, radius, kernelSize);
    });
}

// Encodes an image matrix to PNG or JPG bytes.
bool TryEncodeImage(const cv::Mat& input, EEncodingType encoding, std::vector<unsigned char>& output)
{
    const char* extension = encoding == JPG ? ".jpg" : ".png";
    return cv::imencode(extension, input, output);
}

// Full processing pipeline: decode -> optional blur -> encode.
bool TryProcessImage(
    const unsigned char* input,
    int inputLength,
    EEncodingType encoding,
    bool blur,
    std::vector<unsigned char>& output)
{
    cv::Mat image;
    if (!TryDecodeImage(input, inputLength, image))
    {
        return false;
    }

    // Optionally blur the image using multi-core ROI tiling.
    if (blur)
    {
        cv::Mat blurred;
        ApplyGaussianBlur(image, blurred);
        image = blurred;
    }

    return TryEncodeImage(image, encoding, output);
}
