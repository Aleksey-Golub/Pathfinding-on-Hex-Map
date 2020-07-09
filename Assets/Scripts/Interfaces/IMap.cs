using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IMap
{
    List<ICell> InitMap(Tilemap tilemap, TileBase impassableTile);
    ICell GetCellByPosition(Vector3Int pos);
    TileBase ImpassableTile { get; set; }
}