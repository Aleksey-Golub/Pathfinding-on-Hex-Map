using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : IMap
{
    private TileBase _impassableTile;

    public List<ICell> Cells;
    public TileBase ImpassableTile { get => _impassableTile; set => _impassableTile = value; }


    public ICell GetCellByPosition(Vector3Int pos)
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            if (Cells[i].Position == pos)
                return Cells[i];
        }

        return null;
    }

    public List<ICell> InitMap(Tilemap tilemap, TileBase impassableTile)
    {
        ImpassableTile = impassableTile;
        Cells = new List<ICell>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            ICell c = new Cell();
            c.Position = pos;
            c.IsPassable = tilemap.GetTile(pos) != impassableTile;

            Cells.Add(c);
        }

        return Cells;
    }
}
