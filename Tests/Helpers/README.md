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

## Magyar
### Cél
Segédosztályok a tesztadatok betöltéséhez és a képek összehasonlításához.

### Fájlok
- `SampleImageData`:
  - Minta fájlok beolvasása a `Tests/Samples` mappából.
  - `-blur` kimeneti fájlnevek előállítása.
  - Content type meghatározása kiterjesztés alapján.
- `SampleBase64Data`:
  - Base64 stringek tárolása kulcs alapján.
- `ImageAssert`:
  - Képek összehasonlítása csatorna- és arány toleranciával.
