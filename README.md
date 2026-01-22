# Image Processing Backend Endpoint

## Overview
This repository contains a simplified image processing backend:
- **Api**: ASP.NET Web API that exposes REST endpoints.
- **Dll**: Native C++ DLL that decodes images, optionally applies a Gaussian blur, and re-encodes them.
- **Tests**: C# test project with sample-driven tests.

## Project Structure
- `Api`: C# Web API project.
- `Dll`: C++ native DLL project (OpenCV based).
- `Tests`: xUnit tests and sample images.

## Prerequisites
- Visual Studio with C++ and .NET workloads.
- .NET SDK 10.0 (preview) or matching SDK used by the solution.
- vcpkg with OpenCV:
  - `vcpkg install opencv4:x64-windows`
  - `vcpkg integrate install`
  - Set `VCPKG_ROOT` to your vcpkg path (e.g. `C:\dev\vcpkg`).

## Build and Run
1. Build the native DLL (x64 configuration).
2. Build and run the API (x64 configuration).
3. Open Swagger UI at `/swagger` in the browser.

The API build copies `Dll.dll` and the required OpenCV runtime DLLs into its output folder.

## Tests
1. Add sample images under `Tests/Samples`.
2. Add base64 strings in `Tests/Helpers/SampleBase64Data.cs`.
3. Run tests from Visual Studio or:
   - `dotnet test Tests/Tests.csproj -c Debug`

Pixel comparisons use tolerances to account for encoder differences. See `Tests/README.md` for details.

## Magyar
### Áttekintés
Ez a repó egy leegyszerűsített képfeldolgozó backendet tartalmaz:
- **Api**: ASP.NET Web API REST végpontokkal.
- **Dll**: natív C++ DLL, amely képet dekódol, opcionálisan Gaussian blur-t alkalmaz, majd újraenkódol.
- **Tests**: C# tesztprojekt minta-alapú tesztekkel.

### Projekt felépítése
- `Api`: C# Web API projekt.
- `Dll`: C++ natív DLL projekt (OpenCV).
- `Tests`: xUnit tesztek és minta képek.

### Követelmények
- Visual Studio C++ és .NET workloadokkal.
- .NET SDK 10.0 (preview) vagy az aktuálisan használt SDK.
- vcpkg + OpenCV:
  - `vcpkg install opencv4:x64-windows`
  - `vcpkg integrate install`
  - Állítsd be a `VCPKG_ROOT` változót (pl. `C:\dev\vcpkg`).

### Build és futtatás
1. Építsd a natív DLL-t (x64 konfiguráció).
2. Építsd és futtasd az API-t (x64 konfiguráció).
3. Nyisd meg a Swagger UI-t a böngészőben: `/swagger`.

Az API build átmásolja a `Dll.dll`-t és a szükséges OpenCV futtatási DLL-eket az output mappába.

### Tesztek
1. Minta képek: `Tests/Samples`.
2. Base64 stringek: `Tests/Helpers/SampleBase64Data.cs`.
3. Futtatás Visual Studio-ból vagy:
   - `dotnet test Tests/Tests.csproj -c Debug`

A pixel összehasonlítás toleranciákat használ. Részletek: `Tests/README.md`.
