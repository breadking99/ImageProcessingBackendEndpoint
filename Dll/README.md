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

## Magyar
### Cél
Natív C++ DLL, amely képet dekódol, opcionálisan Gaussian blur-t alkalmaz, majd újraenkódol.

### Exportok
- `MultipleByTwo(int value)` - egyszerű teszt export.
- `ProcessImage(...)` - dekódol, opcionálisan blur-t alkalmaz, majd enkódol.
- `FreeBuffer(void* p)` - felszabadítja a DLL által foglalt memóriát.

### OpenCV
Az OpenCV kezeli a dekódolást, a blur-t és az enkódolást. A blur ROI csempézést és
párhuzamos feldolgozást használ a CPU magok kihasználásához.

### Fájlok
- `Main.cpp` - DLL belépési pont.
- `Exports.h/.cpp` - exportált C ABI és marshaling.
- `ImageProcessing.h/.cpp` - OpenCV pipeline.
- `TileGrid.h` - ROI csempézés metaadatok.
- `Types.h` - megosztott enum típusok.
- `pch.h/.cpp` - precompiled header a gyorsabb buildhez.
