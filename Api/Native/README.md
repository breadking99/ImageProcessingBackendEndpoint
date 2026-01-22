# Native

## Purpose
P/Invoke wrappers for the native C++ DLL.

## Notes
- `DllNative` exposes `ProcessImage`, `MultipleByTwo`, and `FreeBuffer`.
- `ProcessImage` returns a native buffer that must be freed with `FreeBuffer`.
