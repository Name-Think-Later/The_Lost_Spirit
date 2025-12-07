using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 節點連接管理器，負責管理所有節點間的連接關係和視覺化
/// </summary>
public class NodeConnectionManager : MonoBehaviour
{
    [Header("連接線設定")]
    public Material connectionLineMaterial;
    public Color defaultLineColor = Color.white;
    public Color previewLineColor = Color.yellow;
    public Color invalidLineColor = Color.red;
    public float defaultLineWidth = 5f;
    public AnimationCurve lineWidthCurve = AnimationCurve.Linear(0, 1, 1, 1);
    
    [Header("連接預覽設定")]
    public bool showConnectionPreview = true;
    public float previewAnimationSpeed = 2f;
    
    [Header("循環檢測設定")]
    public bool enableCycleDetection = true;
    public int maxDepthForCycleCheck = 50;
    
    [Header("性能設定")]
    public bool updateConnectionsInLateUpdate = true;
    public float connectionUpdateInterval = 0.02f; // 50 FPS
    public int maxConnectionsPerFrame = 10; // 每幀最多更新的連接數
    
    private List<NodeBase> registeredNodes = new List<NodeBase>();
    private List<NodeConnection> allConnections = new List<NodeConnection>();
    private Dictionary<NodeBase, List<NodeConnection>> nodeConnections = new Dictionary<NodeBase, List<NodeConnection>>();
    
    // 拖拽狀態
    private bool isDraggingConnection = false;
    private NodeConnectionPoint dragStartPoint;
    private GameObject dragPreviewLine;
    private NodeConnectionPoint hoveredConnectionPoint;
    
    // 更新管理
    private float lastUpdateTime = 0f;
    private int currentUpdateIndex = 0;
    
    void Awake()
    {
        // 確保預設材質
        if (connectionLineMaterial == null)
        {
            connectionLineMaterial = new Material(Shader.Find("UI/Default"));
        }
    }
    
    /// <summary>
    /// 使用UI Image來創建連接線（替代LineRenderer）
    /// </summary>
    private GameObject CreateUIConnectionLine(NodeConnection connection)
    {
        // 尋找Canvas作為父物件
        Canvas canvas = FindObjectOfType<Canvas>();
        Transform parentTransform = canvas != null ? canvas.transform : transform;
        
        GameObject lineObject = new GameObject($"UIConnection_{connection.ConnectionId}");
        lineObject.transform.SetParent(parentTransform, false);
        
        // 添加RectTransform
        RectTransform lineRT = lineObject.AddComponent<RectTransform>();
        lineRT.anchorMin = new Vector2(0.5f, 0.5f);
        lineRT.anchorMax = new Vector2(0.5f, 0.5f);
        lineRT.pivot = new Vector2(0.5f, 0.5f);
        
        // 添加Image組件
        UnityEngine.UI.Image lineImage = lineObject.AddComponent<UnityEngine.UI.Image>();
        lineImage.color = defaultLineColor;
        
        // 設置連接線位置和旋轉
                // 使用SimpleConnectionLine方式更新
                SimpleConnectionLine simpleLine = connection.ConnectionLine.GetComponent<SimpleConnectionLine>();
                if (simpleLine != null)
                {
                    simpleLine.UpdateLine();
                }        return lineObject;
    }
    
    /// <summary>
    /// 創建簡單的UI連接線
    /// </summary>
    private GameObject CreateSimpleConnectionLine(NodeConnection connection)
    {
        Transform parentTransform = null;
        
        // 找到連接點所屬的 NodeContainer
        if (connection.OutputPoint?.ParentNode != null)
        {
            // 向上查找到 NodeContainer
            Transform nodeTransform = connection.OutputPoint.ParentNode.transform;
            while (nodeTransform != null)
            {
                // 檢查是否是 NodeContainer (通常命名包含 "NodeContainer" 或有特定組件)
                if (nodeTransform.name.Contains("NodeContainer") || 
                    nodeTransform.GetComponent<RectTransform>() != null && 
                    nodeTransform.parent != null && 
                    nodeTransform.parent.name.Contains("page"))
                {
                    parentTransform = nodeTransform;
                    break;
                }
                nodeTransform = nodeTransform.parent;
            }
        }
        
        // 如果找不到 NodeContainer，回退到原來的邏輯
        if (parentTransform == null)
        {
            // 嘗試找到 "main" 物件
            GameObject mainObject = GameObject.Find("main");
            if (mainObject != null)
            {
                parentTransform = mainObject.transform;
            }
            else
            {
                // 如果找不到 main，則使用 Canvas
                Canvas canvas = FindObjectOfType<Canvas>();
                parentTransform = canvas != null ? canvas.transform : transform;
            }
        }
        
        GameObject lineObject = new GameObject($"SimpleConnection_{connection.ConnectionId}");
        lineObject.transform.SetParent(parentTransform, false);
        
        // 添加SimpleConnectionLine組件
        SimpleConnectionLine connectionLine = lineObject.AddComponent<SimpleConnectionLine>();
        connectionLine.lineColor = defaultLineColor;
        connectionLine.lineWidth = defaultLineWidth;
        
        // 設置連接
        connectionLine.SetConnection(connection.OutputPoint, connection.InputPoint);
        
        // 設置關聯的連接資料，用於點擊中斷
        connectionLine.SetAssociatedConnection(connection);
        
        return lineObject;
    }
    
    void Update()
    {
        if (!updateConnectionsInLateUpdate)
        {
            UpdateConnectionsFrame();
        }
    }
    
    void LateUpdate()
    {
        if (updateConnectionsInLateUpdate)
        {
            UpdateConnectionsFrame();
        }
    }
    
    /// <summary>
    /// 註冊節點
    /// </summary>
    public void RegisterNode(NodeBase node)
    {
        if (node != null && !registeredNodes.Contains(node))
        {
            registeredNodes.Add(node);
            nodeConnections[node] = new List<NodeConnection>();
            Debug.Log($"Node registered: {node.nodeName}");
        }
    }
    
    /// <summary>
    /// 取消註冊節點
    /// </summary>
    public void UnregisterNode(NodeBase node)
    {
        if (node == null) return;
        
        // 移除所有相關連接
        if (nodeConnections.ContainsKey(node))
        {
            var connectionsToRemove = new List<NodeConnection>(nodeConnections[node]);
            foreach (var connection in connectionsToRemove)
            {
                RemoveConnection(connection);
            }
        }
        
        registeredNodes.Remove(node);
        nodeConnections.Remove(node);
        Debug.Log($"Node unregistered: {node.nodeName}");
    }
    
    /// <summary>
    /// 創建連接
    /// </summary>
    public bool CreateConnection(NodeConnectionPoint outputPoint, NodeConnectionPoint inputPoint)
    {
        if (outputPoint == null || inputPoint == null) return false;
        if (!outputPoint.CanConnectTo(inputPoint)) return false;
        
        // 檢查輸入點是否已經有連接（通常輸入點只能有一個連接）
        if (!inputPoint.ParentNode.IsConnectionPointAvailable(inputPoint))
        {
            Debug.LogWarning($"Input point on {inputPoint.ParentNode.nodeName} is not available for connection");
            return false;
        }
        
        // 創建連接數據
        var connection = new NodeConnection(outputPoint, inputPoint);
        
        // 創建視覺化連接線（使用SimpleConnectionLine）
        GameObject connectionLine = CreateSimpleConnectionLine(connection);
        connection.SetConnectionLine(connectionLine);
        
        // 添加到管理列表
        allConnections.Add(connection);
        
        // 更新節點的連接記錄
        if (nodeConnections.ContainsKey(connection.OutputNode))
        {
            nodeConnections[connection.OutputNode].Add(connection);
        }
        if (nodeConnections.ContainsKey(connection.InputNode))
        {
            nodeConnections[connection.InputNode].Add(connection);
        }
        
        // 通知連接點
        outputPoint.AddConnection(connection);
        inputPoint.AddConnection(connection);
        
        // 通知節點
        connection.OutputNode.AddConnection(connection);
        connection.InputNode.AddConnection(connection);
        
        Debug.Log($"Connection created: {connection}");
        return true;
    }
    
    /// <summary>
    /// 移除連接
    /// </summary>
    public bool RemoveConnection(NodeConnection connection)
    {
        if (connection == null) return false;
        if (!allConnections.Contains(connection)) return false;
        
        // 從管理列表移除
        allConnections.Remove(connection);
        
        // 更新節點的連接記錄
        if (connection.OutputNode != null && nodeConnections.ContainsKey(connection.OutputNode))
        {
            nodeConnections[connection.OutputNode].Remove(connection);
        }
        if (connection.InputNode != null && nodeConnections.ContainsKey(connection.InputNode))
        {
            nodeConnections[connection.InputNode].Remove(connection);
        }
        
        // 通知連接點
        if (connection.OutputPoint != null)
        {
            connection.OutputPoint.RemoveConnection(connection);
        }
        if (connection.InputPoint != null)
        {
            connection.InputPoint.RemoveConnection(connection);
        }
        
        // 通知節點
        if (connection.OutputNode != null)
        {
            connection.OutputNode.RemoveConnection(connection);
        }
        if (connection.InputNode != null)
        {
            connection.InputNode.RemoveConnection(connection);
        }
        
        // 清理連接
        connection.Cleanup();
        
        Debug.Log($"Connection removed: {connection}");
        return true;
    }
    
    /// <summary>
    /// 開始拖拽連接
    /// </summary>
    public void StartConnectionDrag(NodeConnectionPoint startPoint, Vector2 mousePosition)
    {
        if (startPoint == null) return;
        
        isDraggingConnection = true;
        dragStartPoint = startPoint;
        
        // 創建預覽連接線
        CreateDragPreviewLine(startPoint.GetWorldPosition(), mousePosition);
    }
    
    /// <summary>
    /// 更新拖拽連接
    /// </summary>
    public void UpdateConnectionDrag(Vector2 mousePosition)
    {
        if (!isDraggingConnection || dragPreviewLine == null) return;
        
        // 更新預覽線條
        UpdateDragPreviewLine(dragStartPoint.GetWorldPosition(), mousePosition);
    }
    
    /// <summary>
    /// 結束拖拽連接
    /// </summary>
    public void EndConnectionDrag(bool connectionCreated)
    {
        isDraggingConnection = false;
        dragStartPoint = null;
        hoveredConnectionPoint = null;
        
        // 清理預覽線條
        if (dragPreviewLine != null)
        {
            DestroyImmediate(dragPreviewLine);
            dragPreviewLine = null;
        }
    }
    
    /// <summary>
    /// 顯示連接預覽
    /// </summary>
    public void ShowConnectionPreview(NodeConnectionPoint targetPoint)
    {
        if (!showConnectionPreview) return;
        
        hoveredConnectionPoint = targetPoint;
        
        // 更新預覽線條顏色
        if (dragPreviewLine != null)
        {
            LineRenderer lr = dragPreviewLine.GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.startColor = lr.endColor = previewLineColor;
            }
        }
    }
    
    /// <summary>
    /// 隱藏連接預覽
    /// </summary>
    public void HideConnectionPreview()
    {
        hoveredConnectionPoint = null;
        
        // 恢復預覽線條顏色
        if (dragPreviewLine != null)
        {
            LineRenderer lr = dragPreviewLine.GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.startColor = lr.endColor = defaultLineColor;
            }
        }
    }
    
    /// <summary>
    /// 連接點懸停事件
    /// </summary>
    public void OnConnectionPointHover(NodeConnectionPoint point, bool isHovering)
    {
        // 可以在此處添加懸停效果邏輯
        if (isDraggingConnection && isHovering)
        {
            if (dragStartPoint != null && dragStartPoint.CanConnectTo(point))
            {
                ShowConnectionPreview(point);
            }
        }
        else if (!isHovering && hoveredConnectionPoint == point)
        {
            HideConnectionPreview();
        }
    }
    
    /// <summary>
    /// 檢查是否會創建循環引用
    /// </summary>
    public bool WouldCreateCycle(NodeBase fromNode, NodeBase toNode)
    {
        if (!enableCycleDetection) return false;
        if (fromNode == null || toNode == null) return false;
        if (fromNode == toNode) return true;
        
        // 使用深度優先搜索檢查循環
        var visited = new HashSet<NodeBase>();
        return HasPathBetweenNodes(toNode, fromNode, visited, 0);
    }
    
    /// <summary>
    /// 檢查兩個節點之間是否存在路徑
    /// </summary>
    private bool HasPathBetweenNodes(NodeBase from, NodeBase to, HashSet<NodeBase> visited, int depth)
    {
        if (depth > maxDepthForCycleCheck) return false; // 防止無限遞迴
        if (from == to) return true;
        if (visited.Contains(from)) return false;
        
        visited.Add(from);
        
        // 檢查所有輸出連接
        var outputConnections = GetNodeConnections(from).Where(c => c.OutputNode == from);
        foreach (var connection in outputConnections)
        {
            if (connection.InputNode != null)
            {
                if (HasPathBetweenNodes(connection.InputNode, to, visited, depth + 1))
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    /// <summary>
    /// 獲取節點的所有連接
    /// </summary>
    public List<NodeConnection> GetNodeConnections(NodeBase node)
    {
        if (node == null || !nodeConnections.ContainsKey(node))
            return new List<NodeConnection>();
            
        return new List<NodeConnection>(nodeConnections[node]);
    }
    
    /// <summary>
    /// 獲取所有連接
    /// </summary>
    public List<NodeConnection> GetAllConnections()
    {
        return new List<NodeConnection>(allConnections);
    }
    
    /// <summary>
    /// 清除所有連接
    /// </summary>
    public void ClearAllConnections()
    {
        var connectionsToRemove = new List<NodeConnection>(allConnections);
        foreach (var connection in connectionsToRemove)
        {
            RemoveConnection(connection);
        }
        
        Debug.Log("All connections cleared");
    }
    
    /// <summary>
    /// 更新連接線條（分幀處理）
    /// </summary>
    private void UpdateConnectionsFrame()
    {
        if (Time.time - lastUpdateTime < connectionUpdateInterval) return;
        
        lastUpdateTime = Time.time;
        
        if (allConnections.Count == 0) return;
        
        int connectionsToUpdate = Mathf.Min(maxConnectionsPerFrame, allConnections.Count);
        
        for (int i = 0; i < connectionsToUpdate; i++)
        {
            int index = (currentUpdateIndex + i) % allConnections.Count;
            var connection = allConnections[index];
            
            if (connection.IsValid() && connection.ConnectionLine != null)
            {
                UpdateConnectionLine(connection);
            }
            else if (!connection.IsValid())
            {
                // 移除無效連接
                RemoveConnection(connection);
                break; // 重新開始循環，因為列表已改變
            }
        }
        
        currentUpdateIndex = (currentUpdateIndex + connectionsToUpdate) % Mathf.Max(1, allConnections.Count);
    }
    
    /// <summary>
    /// 創建連接線條
    /// </summary>
    private GameObject CreateConnectionLine(NodeConnection connection)
    {
        GameObject lineObject = new GameObject($"Connection_{connection.ConnectionId}");
        lineObject.transform.SetParent(transform);
        
        LineRenderer lr = lineObject.AddComponent<LineRenderer>();
        
        // 使用預設的UI相容材質
        if (connectionLineMaterial == null)
        {
            connectionLineMaterial = new Material(Shader.Find("UI/Default"));
        }
        lr.material = connectionLineMaterial;
        
        lr.startColor = lr.endColor = defaultLineColor;
        lr.startWidth = defaultLineWidth;
        lr.endWidth = defaultLineWidth;
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        
        // 設置排序層為預設，並調整順序
        lr.sortingLayerName = "Default";
        lr.sortingOrder = 1; // 在背景前面，UI後面
        
        // 設置貝塞爾曲線點
        UpdateConnectionLine(connection);
        
        return lineObject;
    }
    
    /// <summary>
    /// 更新連接線條位置
    /// </summary>
    private void UpdateConnectionLine(NodeConnection connection)
    {
        if (connection.ConnectionLine == null) return;
        
        // 嘗試LineRenderer方式
        LineRenderer lr = connection.ConnectionLine.GetComponent<LineRenderer>();
        if (lr != null)
        {
            Vector3 startPos = connection.GetStartWorldPosition();
            Vector3 endPos = connection.GetEndWorldPosition();
            
            lr.SetPosition(0, startPos);
            lr.SetPosition(1, endPos);
            
            float distance = Vector3.Distance(startPos, endPos);
            float widthMultiplier = lineWidthCurve.Evaluate(distance / 100f);
            lr.startWidth = defaultLineWidth * widthMultiplier;
            lr.endWidth = defaultLineWidth * widthMultiplier;
        }
        else
        {
            // 使用SimpleConnectionLine方式
            SimpleConnectionLine simpleLine = connection.ConnectionLine.GetComponent<SimpleConnectionLine>();
            if (simpleLine != null)
            {
                simpleLine.UpdateLine();
            }
        }
    }
    
    /// <summary>
    /// 創建拖拽預覽線條
    /// </summary>
    private void CreateDragPreviewLine(Vector3 startPos, Vector3 endPos)
    {
        if (dragPreviewLine != null)
        {
            DestroyImmediate(dragPreviewLine);
        }
        
        // 使用 Canvas 作為父物件
        Canvas canvas = FindObjectOfType<Canvas>();
        Transform parentTransform = canvas != null ? canvas.transform : transform;
        
        dragPreviewLine = new GameObject("DragPreviewLine");
        dragPreviewLine.transform.SetParent(parentTransform, false);
        
        // 使用 SimpleConnectionLine 而不是 LineRenderer
        SimpleConnectionLine previewLine = dragPreviewLine.AddComponent<SimpleConnectionLine>();
        previewLine.lineColor = previewLineColor;
        previewLine.lineWidth = defaultLineWidth * 0.8f; // 稍微細一點
        
        // 重要：禁用預覽線條的 raycast，避免干擾連接檢測
        UnityEngine.UI.Image previewImage = dragPreviewLine.GetComponent<UnityEngine.UI.Image>();
        if (previewImage != null)
        {
            previewImage.raycastTarget = false;
        }
        
        // 設置起始和結束位置
        previewLine.SetPositions(startPos, endPos);
    }
    
    /// <summary>
    /// 更新拖拽預覽線條
    /// </summary>
    private void UpdateDragPreviewLine(Vector3 startPos, Vector3 endPos)
    {
        if (dragPreviewLine == null) return;
        
        SimpleConnectionLine previewLine = dragPreviewLine.GetComponent<SimpleConnectionLine>();
        if (previewLine != null)
        {
            previewLine.SetPositions(startPos, endPos);
        }
    }
    
    /// <summary>
    /// 獲取統計信息
    /// </summary>
    public string GetConnectionStats()
    {
        return $"Registered Nodes: {registeredNodes.Count}, Active Connections: {allConnections.Count}";
    }
}