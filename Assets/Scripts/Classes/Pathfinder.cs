using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : IPathFinder
{
	public IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd, IMap map)
	{
        // Шаг 1: 2 списка вершин — ожидающие рассмотрения и уже рассмотренныеs.
        List<ICell> closedSet = new List<ICell>();
        List<ICell> openSet = new List<ICell>();

        // Шаг 2: добавить старт в список.
        ICell startNode = new Cell()
        {
            Position = cellStart.Position,
            CameFrom = null,
            PathLengthFromStart = 0,
            HeuristicEstimatePathLength = GetHeuristicPathLength(cellStart.Position, cellEnd.Position)
        };

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // Шаг 3: взять точку с наименьшим F.
            var currentCell = openSet.OrderBy(cell =>
              cell.EstimateFullPathLength).First();

            // Шаг 4: вернуть путь, если мы нашли конец.
            if (currentCell.Position == cellEnd.Position)
                return GetPathForCell(currentCell);

            // Шаг 5: перенос между списками.
            openSet.Remove(currentCell);
            closedSet.Add(currentCell);

            // Шаг 6: пройти циклом по соседним точкам.
            foreach (var neighbourCell in GetNeighbours(currentCell, cellEnd, map))
            {
                // Шаг 7: если сосед уже вписке, пропустить его.
                if (closedSet.Count(cell => cell.Position == neighbourCell.Position) > 0)
                    continue;

                var openCell = openSet.FirstOrDefault(cell =>
                  cell.Position == neighbourCell.Position);

                // Шаг 8: если сосед не в открытом списке - добавим его.
                if (openCell == null)
                    openSet.Add(neighbourCell);
                // иначе сосед уже в списке, проверить, не пришли ли мы более коротким путем
                else if (openCell.PathLengthFromStart > neighbourCell.PathLengthFromStart)
                {
                    openCell.CameFrom = currentCell;
                    openCell.PathLengthFromStart = neighbourCell.PathLengthFromStart;
                }
            }
        }
        // Шаг 9: если пути не существует
        return null;
    }

    private int GetDistanceBetweenNeighbours()
    {
        return 1;
    }

    private int GetHeuristicPathLength(Vector3Int from, Vector3Int to)
    {
        return Math.Abs(from.x - to.x) + Math.Abs(from.y - to.y);
    }

    private List<ICell> GetNeighbours(ICell pathCell, ICell endCell, IMap map)
    {
        var result = new List<ICell>();

        // Поиск соседних клеток.
        ICell[] neighbourCells = new ICell[6];

        if (pathCell.Position.y % 2 == 0)
        {
            neighbourCells[0] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x - 1,  pathCell.Position.y + 1, pathCell.Position.z));
            neighbourCells[1] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x,      pathCell.Position.y + 1, pathCell.Position.z));
            neighbourCells[2] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x + 1,  pathCell.Position.y,     pathCell.Position.z));
            neighbourCells[3] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x,      pathCell.Position.y - 1, pathCell.Position.z));
            neighbourCells[4] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x - 1,  pathCell.Position.y - 1, pathCell.Position.z));
            neighbourCells[5] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x - 1,  pathCell.Position.y,     pathCell.Position.z));
        }
        else
        {
            neighbourCells[0] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x,     pathCell.Position.y + 1, pathCell.Position.z));
            neighbourCells[1] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x + 1, pathCell.Position.y + 1, pathCell.Position.z));
            neighbourCells[2] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x + 1, pathCell.Position.y,     pathCell.Position.z));
            neighbourCells[3] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x + 1, pathCell.Position.y - 1, pathCell.Position.z));
            neighbourCells[4] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x,     pathCell.Position.y - 1, pathCell.Position.z));
            neighbourCells[5] = map.GetCellByPosition(new Vector3Int(pathCell.Position.x - 1, pathCell.Position.y,     pathCell.Position.z));
        }

        foreach (var n in neighbourCells)
        {
            // Проверяем, что клетка не null.
            if (n == null)
                continue;

            // Проверяем проходимость клетки.
            if (map.GetCellByPosition(n.Position).IsPassable == false)
                continue;

            // Заполняем данные для точки маршрута.
            var neighbourCell = new Cell()
            {
                Position = n.Position,
                CameFrom = pathCell,
                PathLengthFromStart = pathCell.PathLengthFromStart + GetDistanceBetweenNeighbours(),
                HeuristicEstimatePathLength = GetHeuristicPathLength(n.Position, endCell.Position)
            };
            result.Add(neighbourCell);
        }
        return result;
    }

    private IList<ICell> GetPathForCell(ICell pathCell)
    {
        var result = new List<ICell>();
        var currentCell = pathCell;
        while (currentCell != null)
        {
            result.Add(currentCell);
            currentCell = currentCell.CameFrom;
        }
        result.Reverse();
        return result;
    }
}
