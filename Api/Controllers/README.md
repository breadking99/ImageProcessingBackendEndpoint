# Controllers

## Purpose
Defines the API endpoints for image processing and utility operations.

## Notes
- `ImageController` handles base64 processing, file conversion, and base64 export.
- The controller returns file responses with the correct content type based on `EEncodingType`.
- File uploads are read asynchronously and endpoints accept a `CancellationToken`.

## Magyar
### Cél
Az API végpontok definíciója a képfeldolgozáshoz és a segéd műveletekhez.

### Megjegyzések
- Az `ImageController` kezeli a base64 feldolgozást, a fájl konverziót és a base64 exportot.
- A controller a `EEncodingType` alapján állítja be a megfelelő content type-ot.
- A fájlok aszinkron módon kerülnek beolvasásra, és a végpontok `CancellationToken`-t is fogadnak.
