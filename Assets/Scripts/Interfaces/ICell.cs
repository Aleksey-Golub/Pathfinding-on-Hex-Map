using UnityEngine;

public interface ICell
{
    /// <summary>
    /// Координаты ячейки на карте.
    /// </summary>
    Vector3Int Position { get; set; }

    /// <summary>
    /// Длина пути от старта (G в А*). 
    /// </summary>
    int PathLengthFromStart { get; set; }

    /// <summary>
    /// Ячейка, из которой пришли в эту ячейку.
    /// </summary>
    ICell CameFrom { get; set; }

    /// <summary>
    /// Примерное расстояние до цели (H в А*). 
    /// </summary>
    int HeuristicEstimatePathLength { get; set; }

    /// <summary>
    /// Ожидаемое полное расстояние до цели (F = G + H в А*).
    /// </summary>
    int EstimateFullPathLength { get; }

    /// <summary>
    /// Проходимость клетки
    /// </summary>
    bool IsPassable { get; set; }
}