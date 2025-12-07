using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 節點基礎類別，所有可連接的節點都應該繼承此類
/// </summary>
public abstract class NodeBase : MonoBehaviour
{
    [Header("節點基本設定")]
    public string nodeName = "Node";
    public NodeType nodeType = NodeType.Unknown;
    
    [Header("連接點設定")]
    public List<NodeConnectionPoint> inputPoints = new List<NodeConnectionPoint>();
    public List<NodeConnectionPoint> outputPoints = new List<NodeConnectionPoint>();
    
    [Header("視覺設定")]
    public RectTransform nodeRectTransform;
    public Color nodeColor = Color.white;
    
    protected NodeConnectionManager connectionManager;
    protected List<NodeConnection> connections = new List<NodeConnection>();
    
    public virtual void Awake()
    {
        if (nodeRectTransform == null)
            nodeRectTransform = GetComponent<RectTransform>();
            
        // 自動尋找子物件中的連接點
        FindConnectionPoints();
    }
    
    public virtual void Start()
    {
        connectionManager = FindObjectOfType<NodeConnectionManager>();
        if (connectionManager == null)
        {
            Debug.LogError("NodeConnectionManager not found in scene!");
        }
        else
        {
            connectionManager.RegisterNode(this);
        }
        
        // 延遲一幀確保所有組件都已初始化
        StartCoroutine(DelayedInitialize());
    }
    
    private System.Collections.IEnumerator DelayedInitialize()
    {
        yield return null;
        InitializeNode();
    }
    
    public virtual void OnDestroy()
    {
        if (connectionManager != null)
        {
            connectionManager.UnregisterNode(this);
        }
    }
    
    /// <summary>
    /// 初始化節點（子類可覆寫）
    /// </summary>
    protected virtual void InitializeNode()
    {
        // 子類可以在此處添加初始化邏輯
    }
    
    /// <summary>
    /// 自動尋找連接點
    /// </summary>
    private void FindConnectionPoints()
    {
        inputPoints.Clear();
        outputPoints.Clear();
        
        NodeConnectionPoint[] allPoints = GetComponentsInChildren<NodeConnectionPoint>();
        
        foreach (var point in allPoints)
        {
            if (point.connectionType == ConnectionPointType.Input)
            {
                inputPoints.Add(point);
            }
            else if (point.connectionType == ConnectionPointType.Output)
            {
                outputPoints.Add(point);
            }
        }
        
        Debug.Log($"Node {nodeName} found {inputPoints.Count} input points and {outputPoints.Count} output points");
    }
    
    /// <summary>
    /// 添加連接
    /// </summary>
    public virtual void AddConnection(NodeConnection connection)
    {
        if (!connections.Contains(connection))
        {
            connections.Add(connection);
            OnConnectionAdded(connection);
        }
    }
    
    /// <summary>
    /// 移除連接
    /// </summary>
    public virtual void RemoveConnection(NodeConnection connection)
    {
        if (connections.Remove(connection))
        {
            OnConnectionRemoved(connection);
        }
    }
    
    /// <summary>
    /// 獲取所有輸入連接
    /// </summary>
    public List<NodeConnection> GetInputConnections()
    {
        return connections.Where(c => c.InputNode == this).ToList();
    }
    
    /// <summary>
    /// 獲取所有輸出連接
    /// </summary>
    public List<NodeConnection> GetOutputConnections()
    {
        return connections.Where(c => c.OutputNode == this).ToList();
    }
    
    /// <summary>
    /// 獲取連接到此節點的所有前置節點
    /// </summary>
    public List<NodeBase> GetPreviousNodes()
    {
        return GetInputConnections().Select(c => c.OutputNode).Where(n => n != null).ToList();
    }
    
    /// <summary>
    /// 獲取此節點連接到的所有後續節點
    /// </summary>
    public List<NodeBase> GetNextNodes()
    {
        return GetOutputConnections().Select(c => c.InputNode).Where(n => n != null).ToList();
    }
    
    /// <summary>
    /// 檢查是否連接到指定節點
    /// </summary>
    public bool IsConnectedTo(NodeBase otherNode)
    {
        return connections.Any(c => c.ContainsNode(otherNode));
    }
    
    /// <summary>
    /// 斷開所有連接
    /// </summary>
    public virtual void DisconnectAll()
    {
        if (connectionManager != null)
        {
            var connectionsToRemove = new List<NodeConnection>(connections);
            foreach (var connection in connectionsToRemove)
            {
                connectionManager.RemoveConnection(connection);
            }
        }
    }
    
    /// <summary>
    /// 斷開與指定節點的連接
    /// </summary>
    public virtual void DisconnectFrom(NodeBase otherNode)
    {
        if (connectionManager != null)
        {
            var connectionsToRemove = connections.Where(c => c.ContainsNode(otherNode)).ToList();
            foreach (var connection in connectionsToRemove)
            {
                connectionManager.RemoveConnection(connection);
            }
        }
    }
    
    /// <summary>
    /// 獲取指定連接點的所有連接
    /// </summary>
    public List<NodeConnection> GetConnectionsForPoint(NodeConnectionPoint point)
    {
        return connections.Where(c => c.ContainsConnectionPoint(point)).ToList();
    }
    
    /// <summary>
    /// 檢查連接點是否可用
    /// </summary>
    public virtual bool IsConnectionPointAvailable(NodeConnectionPoint point)
    {
        // 預設情況下，輸出點可以有多個連接，輸入點只能有一個連接
        if (point.connectionType == ConnectionPointType.Output)
        {
            return true; // 輸出點可以連接多個
        }
        else
        {
            return GetConnectionsForPoint(point).Count == 0; // 輸入點只能有一個連接
        }
    }
    
    /// <summary>
    /// 當連接被添加時調用（子類可覆寫）
    /// </summary>
    protected virtual void OnConnectionAdded(NodeConnection connection)
    {
        Debug.Log($"Connection added to node {nodeName}: {connection}");
    }
    
    /// <summary>
    /// 當連接被移除時調用（子類可覆寫）
    /// </summary>
    protected virtual void OnConnectionRemoved(NodeConnection connection)
    {
        Debug.Log($"Connection removed from node {nodeName}: {connection}");
    }
    
    /// <summary>
    /// 處理節點輸入數據（子類應覆寫）
    /// </summary>
    public virtual void ProcessInput(object data, NodeConnectionPoint fromPoint)
    {
        // 子類應該實現具體的數據處理邏輯
    }
    
    /// <summary>
    /// 發送輸出數據到連接的節點
    /// </summary>
    protected virtual void SendOutput(object data, NodeConnectionPoint fromPoint)
    {
        var outputConnections = GetConnectionsForPoint(fromPoint);
        foreach (var connection in outputConnections)
        {
            if (connection.InputNode != null)
            {
                connection.InputNode.ProcessInput(data, connection.InputPoint);
            }
        }
    }
    
    /// <summary>
    /// 獲取節點的世界位置
    /// </summary>
    public Vector3 GetWorldPosition()
    {
        return nodeRectTransform != null ? nodeRectTransform.position : transform.position;
    }
    
    /// <summary>
    /// 獲取節點的邊界
    /// </summary>
    public Bounds GetBounds()
    {
        if (nodeRectTransform != null)
        {
            Vector3[] corners = new Vector3[4];
            nodeRectTransform.GetWorldCorners(corners);
            
            Vector3 min = corners[0];
            Vector3 max = corners[0];
            
            for (int i = 1; i < 4; i++)
            {
                min = Vector3.Min(min, corners[i]);
                max = Vector3.Max(max, corners[i]);
            }
            
            Vector3 center = (min + max) * 0.5f;
            Vector3 size = max - min;
            
            return new Bounds(center, size);
        }
        
        return new Bounds(transform.position, Vector3.one);
    }
}