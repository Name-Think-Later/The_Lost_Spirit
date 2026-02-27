using System;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 確保 CellType 定義存在
// 如果你有獨立的 CellType.cs 檔案，這裡可以不用重複定義
// 如果沒有，請將 enum 放在 namespace 外面

public class Platformer2DPathfinder : MonoBehaviour
{
    public Dictionary<PointNode, CellType> _points = new Dictionary<PointNode, CellType>();

    Tilemap _tilemap;
    PointGraph _pointGraph;
    GridGraph _gridGraph;
    BoundsInt _localBounds;
    NNConstraint _constraint;

    [Header("參數設定")]
    public int _jumpHeight = 3;
    public int _jumpDistance = 4;
    public float nodeSize = 0.5f;

    [Header("體積偵測")]
    public LayerMask obstacleLayer;
    // 技巧：將寬度設窄一點 (0.5f)，讓 AI 比較容易判定可以通過窄路
    public Vector2 agentSize = new Vector2(0.5f, 0.7f);

    void Awake()
    {
        AstarPath.active.maxNearestNodeDistance = 10;
        _constraint = new NNConstraint { constrainDistance = true };

        _tilemap = GetComponent<Tilemap>();
        _localBounds = _tilemap.cellBounds;

        _pointGraph = AstarPath.active.graphs[0] as PointGraph;
        _gridGraph = AstarPath.active.graphs[1] as GridGraph;

        MapScan();
        JumpNodeConnect();
    }

    void MapScan()
    {
        PointNode prevNode = null;
        _points.Clear();
        foreach (var tilePosition in _localBounds.allPositionsWithin)
        {
            if (!_tilemap.HasTile(tilePosition)) continue;
            var above = tilePosition + Vector3Int.up;
            if (_tilemap.HasTile(above)) continue;

            CellType cellType = GetCellType(above);
            var pos = CellToWorld(above);
            // 節點抬高 0.2
            Vector3 finalPos = pos + Vector3.up * 0.2f;

            AstarPath.active.AddWorkItem(new AstarWorkItem(ctx => {
                _pointGraph.AddNode((Int3)finalPos);
            }));
            AstarPath.active.FlushWorkItems();

            var node = _pointGraph.GetNearest(finalPos, _constraint).node as PointNode;
            if (node == null || _points.ContainsKey(node)) continue;
            _points.Add(node, cellType);

            // 水平連線 (這裡不做體積檢查，假設地面是平的就能走)
            if (prevNode != null)
                GraphNode.Connect(prevNode, node, (uint)(prevNode.position - node.position).costMagnitude);

            prevNode = (cellType.HasFlag(CellType.RightWall) || cellType.HasFlag(CellType.RightEdge)) ? null : node;

            if (cellType.HasFlag(CellType.LeftEdge)) Dig(tilePosition, Vector3Int.left, node);
            if (cellType.HasFlag(CellType.RightEdge)) Dig(tilePosition, Vector3Int.right, node);
        }
    }

    void Dig(Vector3Int tilePos, Vector3Int direction, PointNode node)
    {
        Vector3Int ray = tilePos + direction;
        while (ray.y > _localBounds.yMin)
        {
            if (_tilemap.HasTile(ray + Vector3Int.down)) break;
            ray += Vector3Int.down;
        }
        if (!_tilemap.HasTile(ray + Vector3Int.down)) return;

        var rayPos = CellToWorld(ray) + Vector3.up * 0.2f;
        var rayNode = _pointGraph.GetNearest(rayPos, _constraint).node as PointNode;
        if (rayNode == null) return;

        if (_points.ContainsKey(rayNode)) _points[rayNode] |= CellType.Drop;

        // --- 除錯版 L 型偵測 ---
        Vector3 start = (Vector3)node.position;
        Vector3 end = (Vector3)rayNode.position;
        Vector3 dirHorizontal = direction.x > 0 ? Vector3.right : Vector3.left;

        // 1. 水平檢查
        Vector3 edgePos = start + dirHorizontal * 0.6f;

        // 視覺化：黃色線代表水平偵測
        Debug.DrawLine(start, edgePos, Color.yellow, 2f);

        // 注意：這裡將高度設為 0.1 (極扁)，避免撞到天花板
        RaycastHit2D hitHead = Physics2D.BoxCast(start, new Vector2(0.1f, 0.1f), 0f, dirHorizontal, 0.6f, obstacleLayer);

        if (hitHead.collider != null)
        {
            Debug.LogError($"掉落失敗(水平擋住): {hitHead.collider.name} 在 {hitHead.point}");
            return;
        }

        // 2. 垂直檢查
        float dropHeight = start.y - end.y;

        // 視覺化：洋紅色代表垂直掉落偵測
        Debug.DrawLine(edgePos, edgePos + Vector3.down * dropHeight, Color.magenta, 2f);

        // 注意：這裡將寬度設為 0.2 (極細)，先確保能通再說
        RaycastHit2D hitBody = Physics2D.BoxCast(edgePos, new Vector2(0.2f, agentSize.y), 0f, Vector2.down, dropHeight - 0.2f, obstacleLayer);

        if (hitBody.collider == null)
        {
            // 成功！畫綠線
            Debug.DrawLine(start, end, Color.green, 5f);
            uint cost = (uint)(node.position - rayNode.position).costMagnitude;
            node.AddPartialConnection(rayNode, cost, true, false);
        }
        else
        {
            // 失敗！畫紅線並報錯
            Debug.LogError($"掉落失敗(垂直擋住): {hitBody.collider.name} 位於 {hitBody.point}");
            Debug.DrawLine(edgePos, hitBody.point, Color.red, 2f);
        }
    }

    void JumpNodeConnect()
    {
        foreach (var (point, type) in _points)
        {
            if (type.HasFlag(CellType.RightEdge)) SideScan(point, CellType.LeftEdge);
            if (type.HasFlag(CellType.LeftEdge)) SideScan(point, CellType.RightEdge);
        }
    }

    void SideScan(PointNode node, CellType targetType)
    {
        int inverse = (targetType == CellType.LeftEdge) ? 1 : -1;
        for (int y = -_jumpHeight; y <= _jumpHeight; y++)
        {
            for (int x = 1; x <= _jumpDistance; x++)
            {
                Vector3 offset = new Vector3(x, y) * inverse;
                CheckJump(node, offset, targetType);
            }
        }
    }

    void CheckJump(PointNode node, Vector3 offset, CellType targetType)
    {
        offset.Scale(_tilemap.cellSize);
        Vector3 start = (Vector3)node.position;
        Vector3 end = start + offset;

        // 跳躍依舊維持斜向檢查，但稍微寬容一點 (-0.2f 距離)
        RaycastHit2D hit = Physics2D.BoxCast(start, new Vector2(agentSize.x * 0.8f, agentSize.y), 0f, (end - start).normalized, Vector3.Distance(start, end) - 0.2f, obstacleLayer);

        if (hit.collider != null) return;

        var otherNode = _pointGraph.GetNearest(end, _constraint).node as PointNode;
        if (otherNode == null || Vector3.Distance((Vector3)otherNode.position, end) > 0.3f) return;
        if (!_points.ContainsKey(otherNode) || !_points[otherNode].HasFlag(targetType)) return;

        node.AddPartialConnection(otherNode, (uint)(node.position - otherNode.position).costMagnitude, true, false);
    }

    CellType GetCellType(Vector3Int position)
    {
        CellType cellType = CellType.Normal;
        if (_tilemap.HasTile(position + Vector3Int.left)) cellType |= CellType.LeftWall;
        if (!_tilemap.HasTile(position + Vector3Int.left + Vector3Int.down)) cellType |= CellType.LeftEdge;
        if (_tilemap.HasTile(position + Vector3Int.right)) cellType |= CellType.RightWall;
        if (!_tilemap.HasTile(position + Vector3Int.right + Vector3Int.down)) cellType |= CellType.RightEdge;
        return cellType;
    }

    Vector3 CellToWorld(Vector3Int cell) => _tilemap.CellToWorld(cell) + _tilemap.cellSize / 2;

    void OnDrawGizmos()
    {
        if (_points == null) return;
        foreach (var pair in _points)
        {
            if (pair.Value.HasFlag(CellType.LeftEdge)) Gizmos.color = Color.red;
            else if (pair.Value.HasFlag(CellType.RightEdge)) Gizmos.color = Color.blue;
            else if (pair.Value.HasFlag(CellType.Drop)) Gizmos.color = Color.cyan;
            else Gizmos.color = Color.white;
            Gizmos.DrawSphere((Vector3)pair.Key.position, 0.1f);

            if (pair.Key.connections == null) continue;
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
            foreach (var c in pair.Key.connections)
            {
                Gizmos.DrawLine((Vector3)pair.Key.position, (Vector3)c.node.position);
                Gizmos.DrawSphere(Vector3.Lerp((Vector3)pair.Key.position, (Vector3)c.node.position, 0.2f), 0.04f);
            }
        }
    }
}