using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 新的節點連接控制器，替代原本的MainNodeC系統
/// 提供節點到節點的直接拖拽連接功能
/// </summary>
public class NodeConnectionController : MonoBehaviour
{
    [Header("系統設定")]
    public bool enableConnectionSystem = true;
    public bool showDebugInfo = true;
    
    [Header("預設節點Prefab")]
    public GameObject mainNodeWPrefab;
    public GameObject mainNodeEPrefab;
    public GameObject mainNodeMPrefab;
    
    [Header("UI設定")]
    public Transform nodeContainer;
    public Button addNodeWButton;
    public Button addNodeEButton;
    public Button addNodeMButton;
    public Button clearAllButton;
    public Text statusText;
    
    private NodeConnectionManager connectionManager;
    private List<NodeBase> spawnedNodes = new List<NodeBase>();
    
    void Start()
    {
        InitializeSystem();
        SetupUI();
    }
    
    /// <summary>
    /// 初始化系統
    /// </summary>
    private void InitializeSystem()
    {
        // 尋找或創建連接管理器
        connectionManager = FindObjectOfType<NodeConnectionManager>();
        if (connectionManager == null)
        {
            GameObject managerObj = new GameObject("NodeConnectionManager");
            connectionManager = managerObj.AddComponent<NodeConnectionManager>();
            Debug.Log("Created NodeConnectionManager");
        }
        
        // 設置節點容器
        if (nodeContainer == null)
        {
            nodeContainer = transform;
        }
    }
    
    /// <summary>
    /// 設置UI
    /// </summary>
    private void SetupUI()
    {
        if (addNodeWButton != null)
        {
            addNodeWButton.onClick.AddListener(() => CreateNode(NodeType.MainNodeW));
        }
        
        if (addNodeEButton != null)
        {
            addNodeEButton.onClick.AddListener(() => CreateNode(NodeType.MainNodeE));
        }
        
        if (addNodeMButton != null)
        {
            addNodeMButton.onClick.AddListener(() => CreateNode(NodeType.MainNodeM));
        }
        
        if (clearAllButton != null)
        {
            clearAllButton.onClick.AddListener(ClearAllNodes);
        }
    }
    
    void Update()
    {
        if (showDebugInfo)
        {
            UpdateStatusText();
        }
    }
    
    /// <summary>
    /// 創建節點
    /// </summary>
    public NodeBase CreateNode(NodeType nodeType, Vector3? position = null)
    {
        if (!enableConnectionSystem) return null;
        
        GameObject prefab = GetNodePrefab(nodeType);
        if (prefab == null)
        {
            Debug.LogError($"No prefab found for node type: {nodeType}");
            return null;
        }
        
        // 創建節點
        GameObject nodeObj = Instantiate(prefab, nodeContainer);
        
        // 設置位置
        Vector3 spawnPosition = position ?? GetNextNodePosition();
        nodeObj.transform.position = spawnPosition;
        
        // 確保節點有正確的組件
        NodeBase nodeBase = SetupNodeComponents(nodeObj, nodeType);
        if (nodeBase != null)
        {
            spawnedNodes.Add(nodeBase);
            Debug.Log($"Created node: {nodeType} at position {spawnPosition}");
        }
        
        return nodeBase;
    }
    
    /// <summary>
    /// 獲取節點Prefab，如果沒有預製體則創建基本節點
    /// </summary>
    private GameObject GetNodePrefab(NodeType nodeType)
    {
        GameObject prefab = null;
        
        switch (nodeType)
        {
            case NodeType.MainNodeW:
                prefab = mainNodeWPrefab;
                break;
            case NodeType.MainNodeE:
                prefab = mainNodeEPrefab;
                break;
            case NodeType.MainNodeM:
                prefab = mainNodeMPrefab;
                break;
        }
        
        // 如果沒有預製體，創建基本節點
        if (prefab == null)
        {
            prefab = CreateBasicNodePrefab(nodeType);
        }
        
        return prefab;
    }
    
    /// <summary>
    /// 創建基本節點預製體
    /// </summary>
    private GameObject CreateBasicNodePrefab(NodeType nodeType)
    {
        // 創建基本節點GameObject
        GameObject nodeObj = new GameObject($"BasicNode_{nodeType}");
        
        // 添加RectTransform
        RectTransform rt = nodeObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(100f, 60f);
        
        // 添加背景圖像
        UnityEngine.UI.Image bgImage = nodeObj.AddComponent<UnityEngine.UI.Image>();
        bgImage.color = GetNodeColor(nodeType);
        
        // 添加文字標籤
        GameObject textObj = new GameObject("Label");
        textObj.transform.SetParent(nodeObj.transform, false);
        
        UnityEngine.UI.Text text = textObj.AddComponent<UnityEngine.UI.Text>();
        text.text = nodeType.ToString();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 12;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        
        RectTransform textRT = textObj.GetComponent<RectTransform>();
        textRT.anchorMin = Vector2.zero;
        textRT.anchorMax = Vector2.one;
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;
        
        return nodeObj;
    }
    
    /// <summary>
    /// 根據節點類型獲取顏色
    /// </summary>
    private Color GetNodeColor(NodeType nodeType)
    {
        switch (nodeType)
        {
            case NodeType.MainNodeW:
                return new Color(0.3f, 0.7f, 1f); // 淺藍色
            case NodeType.MainNodeE:
                return new Color(1f, 0.7f, 0.3f); // 橙色
            case NodeType.MainNodeM:
                return new Color(0.7f, 1f, 0.3f); // 淺綠色
            default:
                return Color.gray;
        }
    }
    
    /// <summary>
    /// 設置節點組件
    /// </summary>
    private NodeBase SetupNodeComponents(GameObject nodeObj, NodeType nodeType)
    {
        NodeBase nodeBase = nodeObj.GetComponent<NodeBase>();
        
        // 如果沒有NodeBase組件，根據類型添加
        if (nodeBase == null)
        {
            switch (nodeType)
            {
                case NodeType.MainNodeW:
                    nodeBase = nodeObj.AddComponent<MainNodeW_New>();
                    break;
                case NodeType.MainNodeE:
                    nodeBase = nodeObj.AddComponent<MainNodeE_New>();
                    break;
                case NodeType.MainNodeM:
                    nodeBase = nodeObj.AddComponent<MainNodeM_New>();
                    break;
            }
        }
        
        // 確保有拖拽組件（用於移動節點）
        if (nodeObj.GetComponent<NodeDragHandler>() == null)
        {
            nodeObj.AddComponent<NodeDragHandler>();
        }
        
        // 確保有RectTransform
        if (nodeObj.GetComponent<RectTransform>() == null)
        {
            nodeObj.AddComponent<RectTransform>();
        }
        
        // 設置基本屬性
        if (nodeBase != null)
        {
            nodeBase.nodeName = $"{nodeType}_{spawnedNodes.Count + 1}";
            nodeBase.nodeType = nodeType;
        }
        
        return nodeBase;
    }
    
    /// <summary>
    /// 獲取下一個節點的位置
    /// </summary>
    private Vector3 GetNextNodePosition()
    {
        int nodeCount = spawnedNodes.Count;
        float xOffset = (nodeCount % 4) * 180f; // 4列，間距更大
        float yOffset = -(nodeCount / 4) * 120f; // 每行間距120
        
        // 相對於容器中心的位置
        return new Vector3(xOffset - 270f, yOffset + 60f, 0);
    }
    
    /// <summary>
    /// 清除所有節點
    /// </summary>
    public void ClearAllNodes()
    {
        if (connectionManager != null)
        {
            connectionManager.ClearAllConnections();
        }
        
        foreach (var node in spawnedNodes)
        {
            if (node != null)
            {
                DestroyImmediate(node.gameObject);
            }
        }
        
        spawnedNodes.Clear();
        Debug.Log("All nodes cleared");
    }
    
    /// <summary>
    /// 移除特定節點
    /// </summary>
    public void RemoveNode(NodeBase node)
    {
        if (node == null) return;
        
        // 斷開所有連接
        node.DisconnectAll();
        
        // 從列表移除
        spawnedNodes.Remove(node);
        
        // 銷毀GameObject
        DestroyImmediate(node.gameObject);
        
        Debug.Log($"Node removed: {node.nodeName}");
    }
    
    /// <summary>
    /// 更新狀態文字
    /// </summary>
    private void UpdateStatusText()
    {
        if (statusText == null || connectionManager == null) return;
        
        string status = $"Nodes: {spawnedNodes.Count}\n";
        status += connectionManager.GetConnectionStats();
        
        statusText.text = status;
    }
    
    /// <summary>
    /// 取得所有已生成的節點
    /// </summary>
    public List<NodeBase> GetAllNodes()
    {
        return new List<NodeBase>(spawnedNodes);
    }
    
    /// <summary>
    /// 根據名稱尋找節點
    /// </summary>
    public NodeBase FindNodeByName(string nodeName)
    {
        return spawnedNodes.Find(n => n != null && n.nodeName == nodeName);
    }
    
    /// <summary>
    /// 根據類型獲取節點
    /// </summary>
    public List<NodeBase> GetNodesByType(NodeType nodeType)
    {
        return spawnedNodes.FindAll(n => n != null && n.nodeType == nodeType);
    }
    
    /// <summary>
    /// 保存節點配置到JSON
    /// </summary>
    public string SaveNodesToJson()
    {
        var saveData = new NodeSaveData
        {
            nodes = new List<NodeData>(),
            connections = new List<ConnectionData>()
        };
        
        // 保存節點數據
        foreach (var node in spawnedNodes)
        {
            if (node != null)
            {
                saveData.nodes.Add(new NodeData
                {
                    nodeName = node.nodeName,
                    nodeType = node.nodeType.ToString(),
                    position = node.GetWorldPosition()
                });
            }
        }
        
        // 保存連接數據
        if (connectionManager != null)
        {
            var connections = connectionManager.GetAllConnections();
            foreach (var connection in connections)
            {
                if (connection.IsValid())
                {
                    saveData.connections.Add(new ConnectionData
                    {
                        outputNodeName = connection.OutputNode.nodeName,
                        inputNodeName = connection.InputNode.nodeName,
                        outputPointName = connection.OutputPoint.name,
                        inputPointName = connection.InputPoint.name
                    });
                }
            }
        }
        
        return JsonUtility.ToJson(saveData, true);
    }
    
    /// <summary>
    /// 從JSON加載節點配置
    /// </summary>
    public void LoadNodesFromJson(string jsonData)
    {
        try
        {
            ClearAllNodes();
            
            var saveData = JsonUtility.FromJson<NodeSaveData>(jsonData);
            if (saveData == null) return;
            
            // 加載節點
            foreach (var nodeData in saveData.nodes)
            {
                if (System.Enum.TryParse(nodeData.nodeType, out NodeType nodeType))
                {
                    var node = CreateNode(nodeType, nodeData.position);
                    if (node != null)
                    {
                        node.nodeName = nodeData.nodeName;
                    }
                }
            }
            
            // 等待一幀讓節點初始化完成
            StartCoroutine(LoadConnectionsCoroutine(saveData.connections));
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load nodes from JSON: {e.Message}");
        }
    }
    
    /// <summary>
    /// 延遲加載連接
    /// </summary>
    private System.Collections.IEnumerator LoadConnectionsCoroutine(List<ConnectionData> connections)
    {
        yield return null; // 等待一幀
        
        // 加載連接
        foreach (var connectionData in connections)
        {
            var outputNode = FindNodeByName(connectionData.outputNodeName);
            var inputNode = FindNodeByName(connectionData.inputNodeName);
            
            if (outputNode != null && inputNode != null)
            {
                // 尋找對應的連接點
                var outputPoint = outputNode.outputPoints.Find(p => p.name == connectionData.outputPointName);
                var inputPoint = inputNode.inputPoints.Find(p => p.name == connectionData.inputPointName);
                
                if (outputPoint != null && inputPoint != null && connectionManager != null)
                {
                    connectionManager.CreateConnection(outputPoint, inputPoint);
                }
            }
        }
    }
    
    [System.Serializable]
    public class NodeSaveData
    {
        public List<NodeData> nodes;
        public List<ConnectionData> connections;
    }
    
    [System.Serializable]
    public class NodeData
    {
        public string nodeName;
        public string nodeType;
        public Vector3 position;
    }
    
    [System.Serializable]
    public class ConnectionData
    {
        public string outputNodeName;
        public string inputNodeName;
        public string outputPointName;
        public string inputPointName;
    }
}