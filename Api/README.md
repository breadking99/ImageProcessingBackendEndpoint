# Api Project

## Purpose
ASP.NET Web API that exposes image processing endpoints and proxies requests to the native DLL.

## Endpoints
- `GET /image/test/multiple-by-two`
  - Query: `value` (int)
  - Returns `{ input, output }`
- `POST /image/processing`
  - Query: `base64`, `encoding`, `blur`
  - Returns processed image bytes as a file response.
- `POST /image/converting`
  - Form: `file`
  - Query: `encoding`, `blur`
  - Returns processed image bytes as a file response.
- `POST /image/to-base64`
  - Form: `file`
  - Returns base64 string of the uploaded image.

## Encoding
`EEncodingType` supports:
- `PNG`
- `JPG`

The `encoding` query decides the output format and content type.

## Native Interop
The controller calls `Api.Native.DllNative` to invoke the native `ProcessImage` function.
