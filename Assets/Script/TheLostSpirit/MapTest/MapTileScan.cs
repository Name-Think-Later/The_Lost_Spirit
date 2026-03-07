using System;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTileScan : MonoBehaviour
{

    public Dictionary<PointNode, CellType> _points = new Dictionary<PointNode, CellType>();

    Tilemap _tilemap;
    PointGraph _pointGraph;
    GridGraph _gridGraph;
    BoundsInt _localBounds;
    Bounds _worldBounds;
    NNConstraint _constraint;

    readonly int _jumpHeight = 3;
    readonly int _jumpDistance = 4;

    float nodeSize = 0.5f;

    void Awake()
    {
        AstarPath.active.maxNearestNodeDistance = 5;
        _constraint = new NNConstraint { constrainDistance = true };

        _tilemap = GetComponent<Tilemap>();
        _localBounds = _tilemap.cellBounds;

        Vector3 worldCenter = _tilemap.CellToWorld(Vector3Int.CeilToInt(_localBounds.center));
        Vector3 worldSize = Vector3Int.CeilToInt((Vector3)_localBounds.size * _tilemap.cellSize.x * (1 / nodeSize));
        _worldBounds = new Bounds(worldCenter, worldSize);

        _pointGraph = AstarPath.active.graphs[1] as PointGraph;
        _gridGraph = AstarPath.active.graphs[2] as GridGraph;

        MapScan();
        JumpNodeConnect();

        Debug.Log(_points.Count);
        Debug.Log(_pointGraph.nodeCount);
    }

    void MapScan()
    {
        PointNode prevNode = null;
        foreach (var tilePosition in _localBounds.allPositionsWithin)
        {
            if (!_tilemap.HasTile(tilePosition)) continue;

            var above = tilePosition + Vector3Int.up;

            if (_tilemap.HasTile(above)) continue;


            CellType cellType = GetCellType(above);

            var pos = CellToWorld(above);

            AstarPath.active.AddWorkItem(new AstarWorkItem(ctx => {
                _pointGraph.AddNode((Int3)pos);
            }));
            AstarPath.active.FlushWorkItems();

            var node = _pointGraph.GetNearest(pos, _constraint).node as PointNode;
            _points.Add(node, cellType);


            if (prevNode != null)
            {
                GraphNode.Connect(prevNode, node, (uint)(prevNode.position - node.position).costMagnitude);
            }

            prevNode =
                cellType.HasFlag(CellType.RightWall) || cellType.HasFlag(CellType.RightEdge) ? null : node;

            if (cellType.HasFlag(CellType.LeftEdge))
                Dig(tilePosition, Vector3Int.left);
            if (cellType.HasFlag(CellType.RightEdge))
                Dig(tilePosition, Vector3Int.right);

            void Dig(Vector3Int ray, Vector3Int edgeDirection)
            {
                ray += edgeDirection * 2;
                if (!_tilemap.HasTile(ray + Vector3Int.down))
                {
                    while (ray.y > _localBounds.yMin)
                    {
                        if (_tilemap.HasTile(ray + Vector3Int.down)) break;
                        ray += Vector3Int.down;
                    }
                }

                var rayPos = CellToWorld(ray);
                var rayNode = _pointGraph.GetNearest(rayPos, _constraint).node as PointNode;

                if (rayNode == null) return;
                _points[rayNode] |= CellType.Drop;

                GraphNode.Connect(node, rayNode, (uint)(node.position - rayNode.position).costMagnitude);
            }
        }
    }

    void JumpNodeConnect()
    {
        foreach (var (point, type) in _points)
        {
            if (type.HasFlag(CellType.RightEdge))
            {
                SideScan(point, CellType.LeftEdge);
            }
            if (type.HasFlag(CellType.LeftEdge))
            {
                SideScan(point, CellType.RightEdge);
            }
        }

        void SideScan(PointNode node, CellType cellType)
        {
            int inverse = cellType == CellType.LeftEdge ? 1 : -1;

            for (int y = 0; y < _jumpHeight; y++)
            {
                for (int x = 0; x < _jumpDistance - y; x++)
                {
                    Vector3 vector = new Vector3(x + 2, y + 1) * inverse;
                    CheckNode(vector);
                }
            }

            for (int x = 0; x < _jumpDistance; x++)
            {
                Vector3 vector = new Vector3(x + 2, 0) * inverse;
                CheckNode(vector);
            }

            for (int y = 0; y > -_jumpHeight; y--)
            {
                for (int x = 0; x < _jumpDistance + y; x++)
                {
                    Vector3 vector = new Vector3(x + 2, y - 1) * inverse;
                    CheckNode(vector);
                }
            }

            void CheckNode(Vector3 offset)
            {
                offset.Scale(_tilemap.cellSize);
                Vector3 position = (Vector3)node.position + offset;
                var otherNode = _pointGraph.GetNearest(position, _constraint).node as PointNode;

                if (otherNode == null) return;
                if ((Vector3)otherNode.position != position) return;
                if (!_points[otherNode].HasFlag(cellType)) return;

                GraphNode.Connect(node, otherNode, (uint)(node.position - otherNode.position).costMagnitude);
            }
        }
    }
    void OnDrawGizmos()
    {
        if (_points == null || _points.Count == 0) return;

        foreach (var kvp in _points)
        {
            PointNode node = kvp.Key;
            CellType type = kvp.Value;
         
            Gizmos.color = GetColorForCellType(type);
            Vector3 worldPos = (Vector3)node.position;
            Gizmos.DrawSphere(worldPos, 0.15f);

            if (node.connections != null)
            {
                foreach (var connection in node.connections)
                {
                    Gizmos.color = Color.green; 

                    if (Vector3.Distance(worldPos, (Vector3)connection.node.position) > 1.2f)
                    {
                        Gizmos.color = Color.cyan; 
                    }

                    Gizmos.DrawLine(worldPos, (Vector3)connection.node.position);
                }
            }
        }
    }

    
    Color GetColorForCellType(CellType type)
    {
        if (type.HasFlag(CellType.LeftEdge) || type.HasFlag(CellType.RightEdge)) return Color.red; 
        if (type.HasFlag(CellType.Drop)) return Color.yellow; 
        return Color.white; 
    }

    CellType GetCellType(Vector3Int position)
    {

        CellType cellType = CellType.Normal;

        var left = position + Vector3Int.left;
        if (_tilemap.HasTile(left)) cellType |= CellType.LeftWall;

        var leftBelow = left + Vector3Int.down;
        if (!_tilemap.HasTile(leftBelow)) cellType |= CellType.LeftEdge;

        var right = position + Vector3Int.right;
        if (_tilemap.HasTile(right)) cellType |= CellType.RightWall;

        var rightBelow = right + Vector3Int.down;
        if (!_tilemap.HasTile(rightBelow)) cellType |= CellType.RightEdge;

        return cellType;
    }

    Vector3 CellToWorld(Vector3Int cell)
    {
        return _tilemap.CellToWorld(cell) + _tilemap.cellSize / 2;
    }

    public void Scan()
    {
        _gridGraph.center = _worldBounds.center;
        _gridGraph.SetDimensions((int)_worldBounds.size.x, (int)_worldBounds.size.y, nodeSize);
        _gridGraph.Scan();
        Debug.Log($"worldCenter: {_worldBounds.center}, worldSize: {_worldBounds.size}");
    }

    public void OnEnterRoom(object sender, EventArgs args)
    {
        Scan();
    }
}




public enum CellType
{
    Normal = 0x0,  //00000
    Drop = 0x1,  //00001
    LeftEdge = 0x2,  //00010
    LeftWall = 0x4,  //00100
    RightEdge = 0x8,  //01000
    RightWall = 0x10, //10000
}