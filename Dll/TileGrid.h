#pragma once

// Tile layout metadata for ROI-based processing.
struct TileGrid
{
    int tileSize{};
    int tilesX{};
    int tilesY{};
    int totalTiles{};
};