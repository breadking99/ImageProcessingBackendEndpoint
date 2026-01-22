// ImageProcessing.cpp : OpenCV image pipeline implementation.
#include "pch.h"
#include "ImageProcessing.h"
#include <algorithm>

namespace
{
    // Kernel size used for the Gaussian blur.
    constexpr int kKernelSize = 5;
    // Tile size used to balance parallelism and overhead.
    constexpr int kTileSize = 128;
}

bool ImageProcessor::TryDecodeImage(const unsigned char* input, int inputLength, cv::Mat& output)
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

void ImageProcessor::ApplyGaussianBlur(const cv::Mat& input, cv::Mat& output)
{
    // Use all available CPU cores for OpenCV's parallel backend.
    cv::setNumThreads(cv::getNumberOfCPUs());

    const cv::Size kernelSize(kKernelSize, kKernelSize);
    const int radius = kernelSize.width / 2;

    // For tiny images, a single blur pass is faster than tiling.
    if (input.cols <= kernelSize.width || input.rows <= kernelSize.height)
    {
        cv::GaussianBlur(input, output, kernelSize, 0.0, 0.0, cv::BORDER_DEFAULT);
        return;
    }

    output.create(input.size(), input.type());

    // Split into tiles so threads can work on independent ROIs.
    const TileGrid grid = BuildTileGrid(input, kTileSize);
    cv::parallel_for_(cv::Range(0, grid.totalTiles), [&](const cv::Range& range)
    {
        ProcessTileRange(range, input, output, grid, radius, kernelSize);
    });
}

bool ImageProcessor::TryEncodeImage(const cv::Mat& input, EEncodingType encoding, std::vector<unsigned char>& output)
{
    const char* extension = encoding == JPG ? ".jpg" : ".png";
    return cv::imencode(extension, input, output);
}

TileGrid ImageProcessor::BuildTileGrid(const cv::Mat& input, int tileSize)
{
    TileGrid grid{};
    grid.tileSize = tileSize;
    grid.tilesX = (input.cols + tileSize - 1) / tileSize;
    grid.tilesY = (input.rows + tileSize - 1) / tileSize;
    grid.totalTiles = grid.tilesX * grid.tilesY;
    return grid;
}

cv::Rect ImageProcessor::BuildTileRect(const TileGrid& grid, int tileIndex, const cv::Size& imageSize)
{
    const int tileY = tileIndex / grid.tilesX;
    const int tileX = tileIndex % grid.tilesX;

    const int x = tileX * grid.tileSize;
    const int y = tileY * grid.tileSize;
    const int w = std::min(grid.tileSize, imageSize.width - x);
    const int h = std::min(grid.tileSize, imageSize.height - y);

    return cv::Rect(x, y, w, h);
}

cv::Rect ImageProcessor::ExpandRect(const cv::Rect& rect, int radius, const cv::Size& imageSize)
{
    // Expand the ROI so the blur kernel can sample neighboring pixels.
    const int expandedX = std::max(0, rect.x - radius);
    const int expandedY = std::max(0, rect.y - radius);
    const int expandedW = std::min(imageSize.width - expandedX, rect.width + 2 * radius);
    const int expandedH = std::min(imageSize.height - expandedY, rect.height + 2 * radius);
    return cv::Rect(expandedX, expandedY, expandedW, expandedH);
}

void ImageProcessor::BlurTile(
    const cv::Mat& input,
    cv::Mat& output,
    const cv::Rect& tileRect,
    int radius,
    const cv::Size& kernelSize)
{
    // Blur a slightly expanded tile to avoid seam artifacts at tile edges.
    const cv::Rect expandedRect = ExpandRect(tileRect, radius, input.size());
    const cv::Mat tileInput = input(expandedRect);

    cv::Mat tileBlurred;
    cv::GaussianBlur(tileInput, tileBlurred, kernelSize, 0.0, 0.0, cv::BORDER_DEFAULT);

    // Copy only the valid center region back to the output tile.
    const int offsetX = tileRect.x - expandedRect.x;
    const int offsetY = tileRect.y - expandedRect.y;
    const cv::Rect copyRect(offsetX, offsetY, tileRect.width, tileRect.height);
    tileBlurred(copyRect).copyTo(output(tileRect));
}

void ImageProcessor::ProcessTileRange(
    const cv::Range& range,
    const cv::Mat& input,
    cv::Mat& output,
    const TileGrid& grid,
    int radius,
    const cv::Size& kernelSize)
{
    // Each iteration processes a tile independently.
    for (int idx = range.start; idx < range.end; ++idx)
    {
        const cv::Rect tileRect = BuildTileRect(grid, idx, input.size());
        BlurTile(input, output, tileRect, radius, kernelSize);
    }
}

bool ImageProcessor::TryProcessImage(
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

    // Encode the output in the requested format.
    return TryEncodeImage(image, encoding, output);
}