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
