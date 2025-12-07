using UnityEngine;
using System;

/// <summary>
/// 節點連接數據，表示兩個節點之間的連接關係
/// </summary>
[Serializable]
public class NodeConnection
{
    [SerializeField] private NodeConnectionPoint outputPoint;
    [SerializeField] private NodeConnectionPoint inputPoint;
    [SerializeField] private GameObject connectionLine;
    [SerializeField] private string connectionId;
    
    public NodeConnectionPoint OutputPoint => outputPoint;
    public NodeConnectionPoint InputPoint => inputPoint;
    public GameObject ConnectionLine => connectionLine;
    public string ConnectionId => connectionId;
    
    public NodeBase OutputNode => outputPoint?.ParentNode;
    public NodeBase InputNode => inputPoint?.ParentNode;
    
    /// <summary>
    /// 建立節點連接
    /// </summary>
    public NodeConnection(NodeConnectionPoint output, NodeConnectionPoint input)
    {
        if (output == null || input == null)
        {
            throw new ArgumentNullException("Connection points cannot be null");
        }
        
        if (output.connectionType != ConnectionPointType.Output || 
            input.connectionType != ConnectionPointType.Input)
        {
            throw new ArgumentException("Invalid connection point types");
        }
        
        outputPoint = output;
        inputPoint = input;
        connectionId = GenerateConnectionId();
    }
    
    /// <summary>
    /// 設定連接線GameObject
    /// </summary>
    public void SetConnectionLine(GameObject line)
    {
        connectionLine = line;
    }
    
    /// <summary>
    /// 檢查連接是否有效
    /// </summary>
    public bool IsValid()
    {
        return outputPoint != null && inputPoint != null && 
               outputPoint.ParentNode != null && inputPoint.ParentNode != null;
    }
    
    /// <summary>
    /// 獲取連接的起始世界位置
    /// </summary>
    public Vector3 GetStartWorldPosition()
    {
        return outputPoint?.GetWorldPosition() ?? Vector3.zero;
    }
    
    /// <summary>
    /// 獲取連接的結束世界位置
    /// </summary>
    public Vector3 GetEndWorldPosition()
    {
        return inputPoint?.GetWorldPosition() ?? Vector3.zero;
    }
    
    /// <summary>
    /// 檢查連接是否包含指定節點
    /// </summary>
    public bool ContainsNode(NodeBase node)
    {
        return OutputNode == node || InputNode == node;
    }
    
    /// <summary>
    /// 檢查連接是否包含指定連接點
    /// </summary>
    public bool ContainsConnectionPoint(NodeConnectionPoint point)
    {
        return outputPoint == point || inputPoint == point;
    }
    
    /// <summary>
    /// 獲取另一個連接點
    /// </summary>
    public NodeConnectionPoint GetOtherConnectionPoint(NodeConnectionPoint point)
    {
        if (point == outputPoint) return inputPoint;
        if (point == inputPoint) return outputPoint;
        return null;
    }
    
    /// <summary>
    /// 獲取另一個節點
    /// </summary>
    public NodeBase GetOtherNode(NodeBase node)
    {
        if (node == OutputNode) return InputNode;
        if (node == InputNode) return OutputNode;
        return null;
    }
    
    /// <summary>
    /// 生成唯一連接ID
    /// </summary>
    private string GenerateConnectionId()
    {
        return $"{OutputNode?.name}_{outputPoint?.name}_to_{InputNode?.name}_{inputPoint?.name}_{Time.time}";
    }
    
    /// <summary>
    /// 清理連接
    /// </summary>
    public void Cleanup()
    {
        if (connectionLine != null)
        {
            if (Application.isPlaying)
            {
                UnityEngine.Object.Destroy(connectionLine);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(connectionLine);
            }
            connectionLine = null;
        }
    }
    
    public override string ToString()
    {
        return $"Connection: {OutputNode?.name} -> {InputNode?.name}";
    }
    
    public override bool Equals(object obj)
    {
        if (obj is NodeConnection other)
        {
            return outputPoint == other.outputPoint && inputPoint == other.inputPoint;
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(outputPoint, inputPoint);
    }
}