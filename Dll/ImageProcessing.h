#pragma once

#include "DllExports.h"
#include <opencv2/core.hpp>
#include <vector>

// Decodes an encoded image buffer into an OpenCV matrix.
bool TryDecodeImage(const unsigned char* input, int inputLength, cv::Mat& output);

// Applies a Gaussian blur using ROI tiling and parallel execution.
void ApplyGaussianBlur(const cv::Mat& input, cv::Mat& output);

// Encodes an image matrix to PNG or JPG bytes.
bool TryEncodeImage(const cv::Mat& input, EEncodingType encoding, std::vector<unsigned char>& output);

// Full processing pipeline: decode -> optional blur -> encode.
bool TryProcessImage(
    const unsigned char* input,
    int inputLength,
    EEncodingType encoding,
    bool blur,
    std::vector<unsigned char>& output);
