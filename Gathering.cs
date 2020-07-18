using Godot;
using System;

public class Gathering : Node2D
{
    public enum Terrain
    {
        Grass,
        Dirt,
        Stone
    }

    private TileMap _tileMap;
    private System.Collections.Generic.Dictionary<Vector2, Terrain> _cellTerrains = new System.Collections.Generic.Dictionary<Vector2, Terrain>();

    public override void _Ready()
    {
        _tileMap = GetNode<TileMap>("testlevel/Tile Layer 1");

        // Get metadata for all tiles in the tile set
        Godot.Collections.Dictionary tileMaterials = (Godot.Collections.Dictionary)_tileMap.TileSet.GetMeta("tile_meta");

        // Get the 'material' (terrain) for each tile and add them to this array
        Terrain[] tileTerrains = new Terrain[tileMaterials.Count];

        int i = 0;
        foreach (var key in tileMaterials.Keys)
        {
            if (tileMaterials[key] is Godot.Collections.Dictionary mat)
            {
                tileTerrains[i] = StringToTerrain(mat["material"].ToString());
                i++;
            }
        }

        // Build dictionary for every tile in the tile map and its terrain type
        foreach (Vector2 tilePos in _tileMap.GetUsedCells())
        {
            _cellTerrains.Add(tilePos, tileTerrains[_tileMap.GetCellv(tilePos) - 1]);
        }
    }

    public Terrain GetTerrainAtPos(Vector2 pos)
    {
        Terrain terrain;
        _cellTerrains.TryGetValue(_tileMap.WorldToMap(pos), out terrain);

        return terrain;
    }

    private static Terrain StringToTerrain(string str)
    {
        switch (str)
        {
            case "grass":
                return Terrain.Grass;
            case "dirt":
                return Terrain.Dirt;
            case "stone":
                return Terrain.Stone;
        }

        // else
        return Terrain.Dirt;
    }

    public static string TerrainToString(Terrain terrain)
    {
        switch (terrain)
        {
            case Terrain.Grass:
                return "grass";
            case Terrain.Dirt:
                return "dirt";
            case Terrain.Stone:
                return "stone";
        }

        // else
        return "error";
    }
}
