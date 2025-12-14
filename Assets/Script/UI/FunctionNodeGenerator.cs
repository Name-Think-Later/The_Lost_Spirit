using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 函式節點生成器，負責通過按鈕生成節點物件到背包中
/// </summary>
public class FunctionNodeGenerator : MonoBehaviour
{
    [Header("節點生成設置")]
    public Transform backpackContainer;  // 背包容器
    
    [Header("背包節點預製體")]
    public GameObject backpackNodeC;     // backpack_nodeC 預製體
    public GameObject backpackNodeE;     // backpack_nodeE 預製體  
    public GameObject backpackNodeM;     // backpack_nodeM 預製體
    public GameObject backpackNodeW;     // backpack_nodeW 預製體
    
    [Header("生成選項")]
    public NodeType defaultNodeType = NodeType.MainNodeM;
    public bool randomPosition = true;
    public Vector2 spawnAreaSize = new Vector2(200f, 150f);
    
    [Header("背包設置")]
    public int maxNodesInBackpack = 20;
    
    [Header("即時監控")]
    public bool enableRealTimeMonitoring = true;
    public UnityEngine.UI.Text nodeCountDisplay; // 顯示節點數量的 Text UI
    
    // 背包中的節點列表
    private List<GameObject> backpackNodes = new List<GameObject>();
    
    void Start()
    {
        InitializeGenerator();
        UpdateNodeCountDisplay();
    }
    
    void Update()
    {
        if (enableRealTimeMonitoring)
        {
            MonitorBackpackNodes();
        }
    }
    
    /// <summary>
    /// 初始化生成器
    /// </summary>
    void InitializeGenerator()
    {
        // 如果沒有指定背包容器，嘗試自動找到
        if (backpackContainer == null)
        {
            GameObject backpackObj = GameObject.Find("Backpack");
            if (backpackObj != null)
            {
                backpackContainer = backpackObj.transform;
            }
        }
        
        Debug.Log("FunctionNodeGenerator 初始化完成");
    }
    
    /// <summary>
    /// 即時監控背包節點
    /// </summary>
    void MonitorBackpackNodes()
    {
        if (backpackContainer == null) return;
        
        // 清理已被刪除的節點引用
        CleanupDestroyedNodes();
        
        // 重新計算實際的節點數量
        int actualNodeCount = GetActualBackpackNodeCount();
        
        // 如果實際數量與列表不符，同步更新
        if (actualNodeCount != backpackNodes.Count)
        {
            RefreshBackpackNodeList();
            UpdateNodeCountDisplay();
        }
    }
    
    /// <summary>
    /// 清理已被刪除的節點引用
    /// </summary>
    void CleanupDestroyedNodes()
    {
        for (int i = backpackNodes.Count - 1; i >= 0; i--)
        {
            if (backpackNodes[i] == null)
            {
                backpackNodes.RemoveAt(i);
            }
        }
    }
    
    /// <summary>
    /// 獲取實際的背包節點數量
    /// </summary>
    int GetActualBackpackNodeCount()
    {
        if (backpackContainer == null) return 0;
        
        int count = 0;
        for (int i = 0; i < backpackContainer.childCount; i++)
        {
            Transform child = backpackContainer.GetChild(i);
            // 檢查是否是背包節點（可以根據名稱或組件來判斷）
            if (child.name.StartsWith("backpack_") || child.GetComponent<NodeBase>() != null)
            {
                count++;
            }
        }
        return count;
    }
    
    /// <summary>
    /// 刷新背包節點列表
    /// </summary>
    void RefreshBackpackNodeList()
    {
        if (backpackContainer == null) return;
        
        backpackNodes.Clear();
        
        for (int i = 0; i < backpackContainer.childCount; i++)
        {
            Transform child = backpackContainer.GetChild(i);
            if (child.name.StartsWith("backpack_") || child.GetComponent<NodeBase>() != null)
            {
                backpackNodes.Add(child.gameObject);
            }
        }
        

    }
    
    /// <summary>
    /// 更新節點數量顯示
    /// </summary>
    void UpdateNodeCountDisplay()
    {
        if (nodeCountDisplay != null)
        {
            nodeCountDisplay.text = $"{backpackNodes.Count} / {maxNodesInBackpack}";
            
            // 根據數量改變顏色
            if (backpackNodes.Count >= maxNodesInBackpack)
            {
                nodeCountDisplay.color = Color.red; // 滿了用紅色
            }
            else if (backpackNodes.Count >= maxNodesInBackpack * 0.8f)
            {
                nodeCountDisplay.color = Color.yellow; // 快滿了用黃色
            }
            else
            {
                nodeCountDisplay.color = Color.white; // 正常用白色
            }
        }
    }
    
    /// <summary>
    /// 生成指定類型的節點到背包
    /// </summary>
    public void GenerateNode(NodeType nodeType)
    {
        if (backpackContainer == null)
        {
            Debug.LogWarning("背包容器未指定！");
            return;
        }
        
        if (backpackNodes.Count >= maxNodesInBackpack)
        {
            Debug.LogWarning($"背包已滿！最大容量：{maxNodesInBackpack}");
            return;
        }
        
        GameObject backpackPrefab = GetBackpackNodePrefab(nodeType);
        if (backpackPrefab == null)
        {
            Debug.LogError($"找不到節點類型 {nodeType} 的背包預製體！");
            return;
        }
        
        // 創建背包節點實例
        GameObject newNode = CreateBackpackNode(backpackPrefab, nodeType);
        
        // 添加到背包
        AddNodeToBackpack(newNode);
        
        // 更新顯示
        UpdateNodeCountDisplay();
        
        Debug.Log($"成功生成 {nodeType} 背包節點！當前背包節點數：{backpackNodes.Count}");
    }
    
    /// <summary>
    /// 創建背包節點實例
    /// </summary>
    private GameObject CreateBackpackNode(GameObject prefab, NodeType nodeType)
    {
        GameObject newNode = Instantiate(prefab, backpackContainer);
        
        // 設置節點名稱
        newNode.name = $"backpack_{nodeType}_{backpackNodes.Count + 1}";
        
        // 設置節點類型（如果 backpack_node 有 NodeBase 組件）
        NodeBase nodeBase = newNode.GetComponent<NodeBase>();
        if (nodeBase != null)
        {
            nodeBase.nodeType = nodeType;
            nodeBase.nodeName = newNode.name;
        }
        
        // 確保 NodeDrag 組件存在
        NodeDragHandler dragHandler = newNode.GetComponent<NodeDragHandler>();
        if (dragHandler == null)
        {
            dragHandler = newNode.AddComponent<NodeDragHandler>();
        }
        
        return newNode;
    }
    
    /// <summary>
    /// 根據節點類型獲取對應的背包預製體
    /// </summary>
    private GameObject GetBackpackNodePrefab(NodeType nodeType)
    {
        switch (nodeType)
        {
            case NodeType.MainNodeC: return backpackNodeC;
            case NodeType.MainNodeE: return backpackNodeE;
            case NodeType.MainNodeM: return backpackNodeM;
            case NodeType.MainNodeW: return backpackNodeW;
            default: 
                Debug.LogWarning($"未支援的節點類型: {nodeType}，使用預設 M 節點");
                return backpackNodeM;
        }
    }
    

    
    /// <summary>
    /// 將節點添加到背包
    /// </summary>
    private void AddNodeToBackpack(GameObject node)
    {
        backpackNodes.Add(node);
    }
    

    
    /// <summary>
    /// 生成節點（簡化版）
    /// </summary>
    public void GenerateNode()
    {
        GenerateNode(defaultNodeType);
    }
    
    /// <summary>
    /// 清空背包
    /// </summary>
    public void ClearBackpack()
    {
        foreach (GameObject node in backpackNodes)
        {
            if (node != null)
            {
                DestroyImmediate(node);
            }
        }
        backpackNodes.Clear();
        UpdateNodeCountDisplay();
        Debug.Log("背包已清空");
    }
    
    /// <summary>
    /// 從背包移除節點
    /// </summary>
    public void RemoveNodeFromBackpack(GameObject node)
    {
        if (backpackNodes.Contains(node))
        {
            backpackNodes.Remove(node);
            UpdateNodeCountDisplay();
            Debug.Log("節點已從背包中移除");
        }
    }
    
    /// <summary>
    /// 獲取背包中的節點數量
    /// </summary>
    public int GetBackpackNodeCount()
    {
        return backpackNodes.Count;
    }
    
    // 公開方法：生成特定類型的節點
    [ContextMenu("生成 W 節點")]
    public void GenerateWNode() => GenerateNode(NodeType.MainNodeW);
    
    [ContextMenu("生成 E 節點")]
    public void GenerateENode() => GenerateNode(NodeType.MainNodeE);
    
    [ContextMenu("生成 M 節點")]
    public void GenerateMNode() => GenerateNode(NodeType.MainNodeM);
    
    [ContextMenu("生成 C 節點")]
    public void GenerateCNode() => GenerateNode(NodeType.MainNodeC);
    
    [ContextMenu("清空背包")]
    public void ClearBackpackMenu() => ClearBackpack();
}