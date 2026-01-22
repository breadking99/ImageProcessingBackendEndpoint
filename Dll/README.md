# Dll Project

## Purpose
Native C++ DLL that performs image decoding, optional Gaussian blur, and re-encoding.

## Exports
- `MultipleByTwo(int value)` - simple smoke-test export.
- `ProcessImage(...)` - decodes image bytes, optionally blurs, and encodes output.
- `FreeBuffer(void* p)` - frees buffers allocated by the DLL.

## OpenCV
The DLL uses OpenCV for decoding, blurring, and encoding. The blur uses ROI tiling and
parallel processing to use available CPU cores.

## Files
- `Main.cpp` - DLL entry point.
- `Exports.h/.cpp` - exported C ABI and marshaling.
- `ImageProcessing.h/.cpp` - OpenCV pipeline.
- `TileGrid.h` - ROI tiling metadata.
- `Types.h` - shared enum types.
- `pch.h/.cpp` - precompiled headers for faster builds.
