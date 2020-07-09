using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathviewer : MonoBehaviour
{
    [SerializeField] private TileBase _startPointTile;
    [SerializeField] private TileBase _endPointTile;
    [SerializeField] private TileBase _pathPointTile;
    [SerializeField] private TileBase _impassableTile;
    [SerializeField] private Tilemap _mapForPathfinding;
    [SerializeField] private Tilemap _mapForPathviewing;

    [SerializeField] private TMP_Text _startLabel;
    [SerializeField] private TMP_Text _endLabel;
    [SerializeField] private TMP_Text _pathLengthLabel;

    private Camera _mainCamera;
    private Vector3Int _previousStart;
    private Vector3Int _previousEnd;
    private bool _isStart;
    private bool _isEnd;
    private List<Vector3Int> _pathPointList = new List<Vector3Int>();

    private IMap _map;
    private IPathFinder _pathFinder;

    private void Start()
    {
        _mainCamera = Camera.main;
        _pathFinder = new Pathfinder();
        _map = new Map();
        _map.InitMap(_mapForPathfinding, _impassableTile);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetPathBorder(_startPointTile, ref _previousStart, _previousEnd, ref _isStart, ref _isEnd);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SetPathBorder(_endPointTile, ref _previousEnd, _previousStart, ref _isEnd, ref _isStart);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void ViewPath(IList<ICell> path)
    {
        if (path == null)
            return;
        
        for (int i = 1; i < path.Count - 1; i++)
        {
            _pathPointList.Add(path[i].Position);
            _mapForPathviewing.SetTile(path[i].Position, _pathPointTile);
        }
    }

    private void CleanPath()
    {
        foreach (var p in _pathPointList)
        {
            _mapForPathviewing.SetTile(p, null);
        }

        _pathPointList.Clear();
    }

    private void SetPathBorder(TileBase borderPoint, ref Vector3Int prevBorderPos, Vector3Int prevAnotherBorder, ref bool isSet, ref bool isAnotherSet)
    {
        CleanPath();

        Vector3 clickedWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3Int clickedCellPosition = _mapForPathfinding.WorldToCell(clickedWorldPosition);

        if (_mapForPathfinding.GetTile(clickedCellPosition) != _impassableTile && _mapForPathfinding.GetTile(clickedCellPosition) != null)
        {
            if (clickedCellPosition == prevAnotherBorder)
                isAnotherSet = false;

            _mapForPathviewing.SetTile(prevBorderPos, null);

            _mapForPathviewing.SetTile(clickedCellPosition, borderPoint);
            isSet = true;

            prevBorderPos = clickedCellPosition;
        }

        IList<ICell> path = null;

        if (_isStart && _isEnd)
            path = GetPath();
            
        UpdateUI(path);

        ViewPath(path);
    }

    private void UpdateUI(IList<ICell> path)
    {
        _startLabel.text = _isStart ? $"Start X: {_previousStart.x}, Y: {_previousStart.y}" : $"Start is not set";
        _endLabel.text = _isEnd ? $"End X: {_previousEnd.x}, Y: {_previousEnd.y}" : $"End is not set";
        _pathLengthLabel.text = _isStart && _isEnd && path != null ? $"Path Length:  {path.Count - 1}" : $"Path is not set";
    }

    private IList<ICell> GetPath()
    {
        ICell cellStart = _map.GetCellByPosition(_previousStart);
        ICell cellEnd = _map.GetCellByPosition(_previousEnd);

        IList<ICell> path = _pathFinder.FindPathOnMap(cellStart, cellEnd, _map);

        return path;
    }
}
