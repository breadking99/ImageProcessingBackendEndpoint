# Native

## Purpose
P/Invoke wrappers for the native C++ DLL.

## Notes
- `DllNative` exposes `ProcessImage` and `FreeBuffer`.
- `ProcessImage` returns a native buffer that must be freed with `FreeBuffer`.

## Magyar
### Cél
P/Invoke wrapper a natív C++ DLL-hez.

### Megjegyzések
- A `DllNative` biztosítja a `ProcessImage` és `FreeBuffer` hívásokat.
- A `ProcessImage` natív buffert ad vissza, amit a `FreeBuffer`-rel kell felszabadítani.
