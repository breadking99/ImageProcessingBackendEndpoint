# Tests Project

## Purpose
Validates the API controller behavior and native image processing output.

## Data Sources
- Base64 strings: `Tests/Helpers/SampleBase64Data.cs`
- Sample files: `Tests/Samples`

## Test Classes
- `Base64ToImageTests`:
  - Uses base64 samples and compares output pixels to expected sample files.
- `ChangeEncodingTests`:
  - Converts PNG <-> JPG and compares output pixels to expected files.
- `BlurTests`:
  - Compares blurred output to `-blur` expected files.

## Naming Conventions
- Base64 keys should match sample file names, e.g. `emoji1.png`.
- Blurred outputs use `-blur` suffix, e.g. `emoji1-blur.png`.

## Tolerances
Pixel comparisons use tolerances to handle encoder differences:
- PNG: `0, 0.0` (exact)
- JPG: `12, 0.05` (lossy)

Adjust tolerances in `InlineData` if needed.
