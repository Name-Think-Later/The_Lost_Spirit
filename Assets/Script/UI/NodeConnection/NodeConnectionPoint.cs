using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 節點連接點類型
/// </summary>
public enum ConnectionPointType
{
    Input,   // 輸入點
    Output   // 輸出點
}

/// <summary>
/// 節點連接點組件，負責管理節點上的連接點
/// </summary>
public class NodeConnectionPoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("連接點設定")]
    public ConnectionPointType connectionType;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.cyan;
    public Color connectedColor = Color.green;
    public float connectionRange = 50f; // 自動連接範圍
    
    [Header("視覺設定")]
    public Image connectionImage;
    public GameObject connectionIndicator;
    
    private List<NodeConnection> connections = new List<NodeConnection>();
    private NodeConnectionManager connectionManager;
    private bool isHovered = false;
    
    public NodeBase ParentNode { get; private set; }
    public bool IsConnected => connections.Count > 0;
    public List<NodeConnection> Connections => connections;
    
    void Awake()
    {
        if (connectionImage == null)
            connectionImage = GetComponent<Image>();
            
        ParentNode = GetComponentInParent<NodeBase>();
        connectionManager = FindObjectOfType<NodeConnectionManager>();
        
        if (connectionManager == null)
        {
            Debug.LogError("NodeConnectionManager not found in scene!");
        }
    }
    
    void Start()
    {
        UpdateVisualState();
    }
    
    /// <summary>
    /// 添加連接
    /// </summary>
    public void AddConnection(NodeConnection connection)
    {
        if (!connections.Contains(connection))
        {
            connections.Add(connection);
            UpdateVisualState();
        }
    }
    
    /// <summary>
    /// 移除連接
    /// </summary>
    public void RemoveConnection(NodeConnection connection)
    {
        if (connections.Remove(connection))
        {
            UpdateVisualState();
        }
    }
    
    /// <summary>
    /// 檢查是否可以連接到目標點
    /// </summary>
    public bool CanConnectTo(NodeConnectionPoint target)
    {
        if (target == null || target == this) return false;
        if (target.ParentNode == this.ParentNode) return false; // 不能連接到同一節點
        if (connectionType == target.connectionType) return false; // 輸入只能連輸出，反之亦然
        
        // 檢查是否已經連接
        foreach (var connection in connections)
        {
            if ((connection.InputPoint == this && connection.OutputPoint == target) ||
                (connection.OutputPoint == this && connection.InputPoint == target))
            {
                return false; // 已經連接過了
            }
        }
        
        // 檢查循環引用（使用BFS）
        if (WouldCreateCycle(target))
        {
            return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// 檢查連接是否會造成循環引用
    /// </summary>
    private bool WouldCreateCycle(NodeConnectionPoint target)
    {
        if (connectionManager == null) return false;
        
        // 如果this是輸入點，檢查從target的輸出到this的輸入是否會形成循環
        NodeConnectionPoint startPoint = (connectionType == ConnectionPointType.Input) ? target : this;
        NodeConnectionPoint endPoint = (connectionType == ConnectionPointType.Input) ? this : target;
        
        return connectionManager.WouldCreateCycle(startPoint.ParentNode, endPoint.ParentNode);
    }
    
    /// <summary>
    /// 更新視覺狀態
    /// </summary>
    private void UpdateVisualState()
    {
        if (connectionImage == null) return;
        
        Color targetColor = normalColor;
        
        if (IsConnected)
        {
            targetColor = connectedColor;
        }
        else if (isHovered)
        {
            targetColor = hoverColor;
        }
        
        connectionImage.color = targetColor;
        
        // 更新連接指示器
        if (connectionIndicator != null)
        {
            connectionIndicator.SetActive(IsConnected || isHovered);
        }
    }
    
    /// <summary>
    /// 開始拖拽連線
    /// </summary>
    public void StartConnectionDrag(Vector2 mousePosition)
    {
        if (connectionManager != null)
        {
            connectionManager.StartConnectionDrag(this, mousePosition);
        }
    }
    
    /// <summary>
    /// 嘗試建立連接
    /// </summary>
    public bool TryConnectTo(NodeConnectionPoint target)
    {
        if (!CanConnectTo(target)) return false;
        
        if (connectionManager != null)
        {
            NodeConnectionPoint outputPoint = (connectionType == ConnectionPointType.Output) ? this : target;
            NodeConnectionPoint inputPoint = (connectionType == ConnectionPointType.Input) ? this : target;
            
            return connectionManager.CreateConnection(outputPoint, inputPoint);
        }
        
        return false;
    }
    
    /// <summary>
    /// 斷開所有連接
    /// </summary>
    public void DisconnectAll()
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
    /// 獲取世界位置
    /// </summary>
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        UpdateVisualState();
        
        // 通知連接管理器
        if (connectionManager != null)
        {
            connectionManager.OnConnectionPointHover(this, true);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        UpdateVisualState();
        
        // 通知連接管理器
        if (connectionManager != null)
        {
            connectionManager.OnConnectionPointHover(this, false);
        }
    }
}