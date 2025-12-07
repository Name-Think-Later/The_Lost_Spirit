using UnityEngine;

/// <summary>
/// MainNodeC - 起始節點，只有輸出點，沒有輸入點
/// </summary>
public class MainNodeC_New : NodeBase, IAdvancedNodeTypeProvider
{
    [Header("起始節點設定")]
    [SerializeField] private int outputCount = 1;
    [SerializeField] private bool useSymmetricalLayout = false;
    
    public NodeType GetNodeType()
    {
        return NodeType.MainNodeC;
    }

    public int GetBranchCount()
    {
        return outputCount;
    }
    
    public bool UseSymmetricalLayout()
    {
        return useSymmetricalLayout;
    }
    
    protected override void InitializeNode()
    {
        base.InitializeNode();
        
        nodeName = "MainNodeC_Start";
        nodeType = NodeType.MainNodeC; // 設定正確的節點類型
        
        // 清空自動找到的連接點（因為我們要動態創建）
        inputPoints.Clear();
        outputPoints.Clear();
        
        // 起始節點不需要輸入點，只需要輸出點
        EnsureOutputPoints();
        
        Debug.Log($"MainNodeC起始節點初始化完成，輸出點數量: {outputPoints.Count}");
    }
    
    /// <summary>
    /// 確保有足夠的輸出點
    /// </summary>
    private void EnsureOutputPoints()
    {
        while (outputPoints.Count < outputCount)
        {
            CreateOutputPoint();
        }
    }
    
    /// <summary>
    /// 創建輸出點
    /// </summary>
    private void CreateOutputPoint()
    {
        GameObject outputPointObj = new GameObject($"OutputPoint_{outputPoints.Count + 1}");
        outputPointObj.transform.SetParent(transform, false);
        
        // 設置位置
        RectTransform rt = outputPointObj.AddComponent<RectTransform>();
        float xOffset = GetOutputPointXOffset(outputPoints.Count, outputCount);
        rt.anchoredPosition = new Vector2(xOffset, -35f); // 在節點下方
        rt.sizeDelta = new Vector2(20f, 20f); // 增大連接點尺寸以便點擊
        
        // 設置錨點和位置
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        
        // 添加連接點組件
        NodeConnectionPoint connectionPoint = outputPointObj.AddComponent<NodeConnectionPoint>();
        connectionPoint.connectionType = ConnectionPointType.Output;
        connectionPoint.connectionRange = 50f;
        connectionPoint.normalColor = Color.green; // 設定正常顏色
        
        // 添加視覺效果（綠色輸出點）
        UnityEngine.UI.Image image = outputPointObj.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.green; // 輸出點用綠色
        connectionPoint.connectionImage = image;
        
        // 確保輸出點可以接收raycast事件
        image.raycastTarget = true;
        
        // 添加拖拽處理器（只有輸出點需要）
        ConnectionDragHandler dragHandler = outputPointObj.AddComponent<ConnectionDragHandler>();
        
        outputPoints.Add(connectionPoint);
        
        Debug.Log($"Created output point for {nodeName}, color: {image.color}");
    }
    
    /// <summary>
    /// 計算輸出點的X偏移
    /// </summary>
    private float GetOutputPointXOffset(int index, int totalCount)
    {
        if (totalCount == 1)
        {
            return 0f; // 單個輸出點在中央
        }
        
        if (useSymmetricalLayout)
        {
            // 對稱佈局
            float totalWidth = (totalCount - 1) * 40f;
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
    /// 起始節點不接收輸入，重寫ProcessInput
    /// </summary>
    public override void ProcessInput(object data, NodeConnectionPoint fromPoint)
    {
        // 起始節點不處理輸入
        Debug.LogWarning($"起始節點 {nodeName} 不應該接收輸入數據");
    }
    
    /// <summary>
    /// 發送起始數據到所有輸出點
    /// </summary>
    public void SendStartData(object startData)
    {
        Debug.Log($"起始節點 {nodeName} 發送數據: {startData}");
        
        // 將數據發送到所有輸出點
        foreach (var outputPoint in outputPoints)
        {
            SendOutput(startData, outputPoint);
        }
    }
    
    /// <summary>
    /// 手動觸發起始數據流
    /// </summary>
    [ContextMenu("觸發數據流")]
    public void TriggerDataFlow()
    {
        SendStartData("起始數據");
    }
    
    /// <summary>
    /// 檢查輸入點是否可用（起始節點不允許輸入連接）
    /// </summary>
    public override bool IsConnectionPointAvailable(NodeConnectionPoint point)
    {
        if (point.connectionType == ConnectionPointType.Input)
        {
            return false; // 起始節點不允許輸入連接
        }
        
        return base.IsConnectionPointAvailable(point);
    }
    
    /// <summary>
    /// 在Inspector中驗證數值
    /// </summary>
    void OnValidate()
    {
        outputCount = Mathf.Clamp(outputCount, 1, 6);
        
        if (Application.isPlaying)
        {
            EnsureOutputPoints();
        }
    }
}