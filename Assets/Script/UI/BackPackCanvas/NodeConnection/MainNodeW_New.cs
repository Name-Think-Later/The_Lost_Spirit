using UnityEngine;

/// <summary>
/// 新的MainNodeW實現，使用新的連接系統
/// </summary>
public class MainNodeW_New : NodeBase, IAdvancedNodeTypeProvider
{
    [Header("寬節點特殊設定")]
    [SerializeField] private int branchCount = 2;
    [SerializeField] private bool useSymmetricalLayout = true;
    
    public NodeType GetNodeType()
    {
        return NodeType.MainNodeW;
    }

    public int GetBranchCount()
    {
        return branchCount;
    }
    
    public bool UseSymmetricalLayout()
    {
        return useSymmetricalLayout;
    }
    
    protected override void InitializeNode()
    {
        base.InitializeNode();
        
        nodeName = "MainNodeW";
        nodeType = NodeType.MainNodeW;
        
        // 確保有輸入點
        EnsureInputPoint();
        // 確保有足夠的輸出點
        EnsureOutputPoints();
    }
    
    /// <summary>
    /// 確保有輸入點
    /// </summary>
    private void EnsureInputPoint()
    {
        if (inputPoints.Count == 0)
        {
            CreateInputPoint();
        }
    }
    
    /// <summary>
    /// 創建輸入點
    /// </summary>
    private void CreateInputPoint()
    {
        GameObject inputPointObj = new GameObject("InputPoint");
        inputPointObj.transform.SetParent(transform, false);
        
        // 設置位置（節點上方中央）
        RectTransform rt = inputPointObj.AddComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0f, 35f);
        rt.sizeDelta = new Vector2(20f, 20f); // 增大連接點尺寸以便點擊
        
        // 設置錨點和位置
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        
        // 添加連接點組件
        NodeConnectionPoint connectionPoint = inputPointObj.AddComponent<NodeConnectionPoint>();
        connectionPoint.connectionType = ConnectionPointType.Input;
        connectionPoint.connectionRange = 50f;
        connectionPoint.normalColor = Color.blue; // 設定正常顏色
        
        // 添加視覺效果
        UnityEngine.UI.Image image = inputPointObj.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.blue; // 輸入點用藍色
        connectionPoint.connectionImage = image;
        
        // 確保輸入點可以接收raycast事件
        image.raycastTarget = true;
        
        inputPoints.Add(connectionPoint);
        
        Debug.Log($"Created input point for {nodeName}, color: {image.color}");
    }
    
    /// <summary>
    /// 確保有足夠的輸出點
    /// </summary>
    private void EnsureOutputPoints()
    {
        // 檢查是否需要動態創建更多輸出點
        while (outputPoints.Count < branchCount)
        {
            CreateAdditionalOutputPoint();
        }
    }
    
    /// <summary>
    /// 創建額外的輸出點
    /// </summary>
    private void CreateAdditionalOutputPoint()
    {
        GameObject outputPointObj = new GameObject($"OutputPoint_{outputPoints.Count + 1}");
        outputPointObj.transform.SetParent(transform, false);
        
        // 設置位置（水平分布）
        RectTransform rt = outputPointObj.AddComponent<RectTransform>();
        float xOffset = GetOutputPointXOffset(outputPoints.Count, branchCount);
        rt.anchoredPosition = new Vector2(xOffset, -35f); // 在節點下方
        rt.sizeDelta = new Vector2(15f, 15f);
        
        // 設置錨點和位置
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        
        // 添加連接點組件
        NodeConnectionPoint connectionPoint = outputPointObj.AddComponent<NodeConnectionPoint>();
        connectionPoint.connectionType = ConnectionPointType.Output;
        connectionPoint.connectionRange = 50f;
        connectionPoint.normalColor = Color.green; // 設定正常顏色
        
        // 添加視覺效果
        UnityEngine.UI.Image image = outputPointObj.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.green; // 輸出點用綠色
        connectionPoint.connectionImage = image;
        
        // 確保輸出點可以接收raycast事件
        image.raycastTarget = true;
        
        // 添加拖曳處理器（只有輸出點需要）
        ConnectionDragHandler dragHandler = outputPointObj.AddComponent<ConnectionDragHandler>();
        
        outputPoints.Add(connectionPoint);
    }
    
    /// <summary>
    /// 計算輸出點的X偏移
    /// </summary>
    private float GetOutputPointXOffset(int index, int totalCount)
    {
        if (useSymmetricalLayout)
        {
            // 對稱佈局
            float totalWidth = (totalCount - 1) * 40f; // 間距40單位
            float startX = -totalWidth / 2f;
            return startX + index * 40f;
        }
        else
        {
            // 線性佈局
            return (index - (totalCount - 1) / 2f) * 40f;
        }
    }
    
    /// <summary>
    /// 處理輸入數據
    /// </summary>
    public override void ProcessInput(object data, NodeConnectionPoint fromPoint)
    {
        base.ProcessInput(data, fromPoint);
        
        Debug.Log($"MainNodeW {nodeName} received input: {data}");
        
        // 將輸入數據傳遞到所有輸出點
        foreach (var outputPoint in outputPoints)
        {
            SendOutput(data, outputPoint);
        }
    }
    
    /// <summary>
    /// 在Inspector中驗證數值
    /// </summary>
    void OnValidate()
    {
        branchCount = Mathf.Clamp(branchCount, 1, 6);
        
        if (Application.isPlaying)
        {
            EnsureOutputPoints();
        }
    }
}