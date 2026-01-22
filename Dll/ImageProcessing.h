#pragma once

#include "Types.h"
#include "TileGrid.h"
#include <opencv2/core.hpp>
#include <vector>

// OpenCV-based image processing pipeline.
class ImageProcessor
{
public:
    // Decodes input bytes, optionally blurs, and returns encoded output bytes.
    static bool TryProcessImage(
        const unsigned char* input,
        int inputLength,
        EEncodingType encoding,
        bool blur,
        std::vector<unsigned char>& output);

private:
    // Decodes an encoded image buffer into an OpenCV matrix.
    static bool TryDecodeImage(const unsigned char* input, int inputLength, cv::Mat& output);

    // Encodes an image matrix to PNG or JPG bytes.
    static bool TryEncodeImage(const cv::Mat& input, EEncodingType encoding, std::vector<unsigned char>& output);

    // Applies a Gaussian blur using ROI tiling and parallel execution.
    static void ApplyGaussianBlur(const cv::Mat& input, cv::Mat& output);

    // Calculates the tile grid dimensions for a given image size.
    static TileGrid BuildTileGrid(const cv::Mat& input, int tileSize);

    // Returns the tile rectangle for a given tile index.
    static cv::Rect BuildTileRect(const TileGrid& grid, int tileIndex, const cv::Size& imageSize);

    // Expands the tile rectangle to include the blur kernel radius.
    static cv::Rect ExpandRect(const cv::Rect& rect, int radius, const cv::Size& imageSize);

    // Blurs a single tile with padding and copies the valid region to the output.
    static void BlurTile(
        const cv::Mat& input,
        cv::Mat& output,
        const cv::Rect& tileRect,
        int radius,
        const cv::Size& kernelSize);

    // Executes tile processing for a thread range.
    static void ProcessTileRange(
        const cv::Range& range,
        const cv::Mat& input,
        cv::Mat& output,
        const TileGrid& grid,
        int radius,
        const cv::Size& kernelSize);
};