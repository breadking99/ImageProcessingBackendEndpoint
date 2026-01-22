# Helpers

## Purpose
Utility helpers to load test data and compare images.

## Files
- `SampleImageData`:
  - Reads sample files from `Tests/Samples`.
  - Builds expected blurred file names.
  - Infers content types from extensions.
- `SampleBase64Data`:
  - Stores base64 strings keyed by name.
- `ImageAssert`:
  - Compares images using per-channel and ratio tolerances.
