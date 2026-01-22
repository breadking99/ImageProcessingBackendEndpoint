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
- Base64 keys should match sample file names, e.g. `apple.png`.
- Blurred outputs use `-blur` suffix, e.g. `apple-blur.png`.

## Tolerances
Pixel comparisons use tolerances to handle encoder differences:
- PNG: `0, 0.0` (exact)
- JPG: `12, 0.05` (lossy)

Adjust tolerances in `InlineData` if needed.

## Magyar
### Cél
Az API controller viselkedésének és a natív képfeldolgozás kimenetének ellenőrzése.

### Adatforrások
- Base64 stringek: `Tests/Helpers/SampleBase64Data.cs`
- Minta fájlok: `Tests/Samples`

### Teszt osztályok
- `Base64ToImageTests`:
  - Base64 mintákat használ és a kimeneti pixeleket hasonlítja a várt fájlokhoz.
- `ChangeEncodingTests`:
  - PNG <-> JPG konverzió, pixel összehasonlítással.
- `BlurTests`:
  - Elmosott kimenet összehasonlítása `-blur` fájlokkal.

### Névkonvenciók
- A base64 kulcsok egyezzenek a fájlnevekkel, pl. `apple.png`.
- Az elmosott kimenetek `-blur` utótagot kapnak, pl. `apple-blur.png`.

### Toleranciák
A pixel összehasonlítás toleranciákat használ:
- PNG: `0, 0.0` (pontos)
- JPG: `12, 0.05` (veszteséges)

Szükség esetén módosítsd az `InlineData` értékeket.
