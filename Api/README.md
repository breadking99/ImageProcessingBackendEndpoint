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

## Magyar
### Cél
ASP.NET Web API, amely képfeldolgozó végpontokat ad és a natív DLL-t hívja.

### Végpontok
- `GET /image/test/multiple-by-two`
  - Query: `value` (int)
  - Válasz: `{ input, output }`
- `POST /image/processing`
  - Query: `base64`, `encoding`, `blur`
  - Feldolgozott kép fájl válaszként.
- `POST /image/converting`
  - Form: `file`
  - Query: `encoding`, `blur`
  - Feldolgozott kép fájl válaszként.
- `POST /image/to-base64`
  - Form: `file`
  - Visszaadja a feltöltött kép base64 stringjét.

### Kódolás
`EEncodingType` támogatja:
- `PNG`
- `JPG`

A `encoding` query dönti el az output formátumot és a content type-ot.

### Natív interop
A controller az `Api.Native.DllNative` rétegen keresztül hívja a `ProcessImage` natív függvényt.
