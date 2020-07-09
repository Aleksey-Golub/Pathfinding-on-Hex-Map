using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : ICell
{
    private Vector3Int _position;
    private int _pathLengthFromStart;
    private ICell _cameFrom;
    private int _heuristicEstimatePathLength;
    private bool _isPassable;

    public Vector3Int Position { get => _position; set => _position = value; }
    public int PathLengthFromStart { get => _pathLengthFromStart; set => _pathLengthFromStart = value; }
    public ICell CameFrom { get => _cameFrom; set => _cameFrom = value; }
    public int HeuristicEstimatePathLength { get => _heuristicEstimatePathLength; set => _heuristicEstimatePathLength = value; }
    public int EstimateFullPathLength => PathLengthFromStart + HeuristicEstimatePathLength;
    public bool IsPassable { get => _isPassable; set => _isPassable = value; }
}