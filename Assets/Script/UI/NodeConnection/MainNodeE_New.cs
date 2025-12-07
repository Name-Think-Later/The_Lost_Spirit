using UnityEngine;

/// <summary>
/// 新的MainNodeE實現，使用新的連接系統
/// </summary>
public class MainNodeE_New : NodeBase, IAdvancedNodeTypeProvider
{
    [Header("效果節點特殊設定")]
    [SerializeField] private int branchCount = 2;
    [SerializeField] private bool useSymmetricalLayout = true;
    [SerializeField] private float effectIntensity = 1.0f;
    
    public NodeType GetNodeType()
    {
        return NodeType.MainNodeE;
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
        
        nodeName = "MainNodeE";
        nodeType = NodeType.MainNodeE;
        
        // 效果節點通常有一個輸入和多個輸出
        EnsureInputPoint();
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
        rt.sizeDelta = new Vector2(15f, 15f);
        
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
    }
    
    /// <summary>
    /// 確保有足夠的輸出點
    /// </summary>
    private void EnsureOutputPoints()
    {
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
        
        // 設置位置（水平分布在節點下方）
        RectTransform rt = outputPointObj.AddComponent<RectTransform>();
        float xOffset = GetOutputPointXOffset(outputPoints.Count, branchCount);
        rt.anchoredPosition = new Vector2(xOffset, -35f);
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
            float totalWidth = (totalCount - 1) * 40f;
            float startX = -totalWidth / 2f;
            return startX + index * 40f;
        }
        else
        {
            return (index - (totalCount - 1) / 2f) * 40f;
        }
    }
    
    /// <summary>
    /// 處理輸入數據並應用效果
    /// </summary>
    public override void ProcessInput(object data, NodeConnectionPoint fromPoint)
    {
        base.ProcessInput(data, fromPoint);
        
        Debug.Log($"MainNodeE {nodeName} received input: {data}, applying effect with intensity: {effectIntensity}");
        
        // 應用效果處理
        object processedData = ApplyEffect(data);
        
        // 將處理後的數據傳遞到所有輸出點
        foreach (var outputPoint in outputPoints)
        {
            SendOutput(processedData, outputPoint);
        }
    }
    
    /// <summary>
    /// 應用效果處理
    /// </summary>
    private object ApplyEffect(object inputData)
    {
        // 這裡可以實現具體的效果邏輯
        // 例如：數值修改、狀態變更等
        
        if (inputData is float floatValue)
        {
            return floatValue * effectIntensity;
        }
        else if (inputData is int intValue)
        {
            return Mathf.RoundToInt(intValue * effectIntensity);
        }
        
        return inputData; // 預設情況下返回原數據
    }
    
    /// <summary>
    /// 在Inspector中驗證數值
    /// </summary>
    void OnValidate()
    {
        branchCount = Mathf.Clamp(branchCount, 1, 6);
        effectIntensity = Mathf.Max(0f, effectIntensity);
        
        if (Application.isPlaying)
        {
            EnsureInputPoint();
            EnsureOutputPoints();
        }
    }
}