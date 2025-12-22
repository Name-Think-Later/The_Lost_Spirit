using UnityEngine;

/// <summary>
/// 新的MainNodeM實現，使用新的連接系統
/// </summary>
public class MainNodeM_New : NodeBase, IAdvancedNodeTypeProvider
{
    [Header("主節點特殊設定")]
    [SerializeField] private int branchCount = 1;
    [SerializeField] private bool useSymmetricalLayout = true;
    
    public NodeType GetNodeType()
    {
        return NodeType.MainNodeM;
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
        
        nodeName = "MainNodeM";
        nodeType = NodeType.MainNodeM;
        
        // 主節點通常有一個輸入和一個輸出
        EnsureInputPoint();
        EnsureOutputPoint();
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
    /// 確保有輸出點
    /// </summary>
    private void EnsureOutputPoint()
    {
        if (outputPoints.Count == 0)
        {
            CreateOutputPoint();
        }
    }
    
    /// <summary>
    /// 創建輸出點
    /// </summary>
    private void CreateOutputPoint()
    {
        GameObject outputPointObj = new GameObject("OutputPoint");
        outputPointObj.transform.SetParent(transform, false);
        
        // 設置位置（節點下方中央）
        RectTransform rt = outputPointObj.AddComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0f, -35f);
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
        
        // 添加拖拽處理器（只有輸出點需要）
        ConnectionDragHandler dragHandler = outputPointObj.AddComponent<ConnectionDragHandler>();
        
        outputPoints.Add(connectionPoint);
    }
    
    /// <summary>
    /// 處理輸入數據
    /// </summary>
    public override void ProcessInput(object data, NodeConnectionPoint fromPoint)
    {
        base.ProcessInput(data, fromPoint);
        
        Debug.Log($"MainNodeM {nodeName} received input: {data}");
        
        // 主節點通常直接傳遞數據
        object processedData = ProcessMainNodeLogic(data);
        
        // 將處理後的數據傳遞到輸出點
        if (outputPoints.Count > 0)
        {
            SendOutput(processedData, outputPoints[0]);
        }
    }
    
    /// <summary>
    /// 處理主節點邏輯
    /// </summary>
    private object ProcessMainNodeLogic(object inputData)
    {
        // 這裡可以實現主節點的特殊邏輯
        // 例如：數據驗證、轉換、記錄等
        
        Debug.Log($"MainNodeM processing data: {inputData}");
        
        // 預設情況下直接返回輸入數據
        return inputData;
    }
    
    /// <summary>
    /// 在Inspector中驗證數值
    /// </summary>
    void OnValidate()
    {
        branchCount = Mathf.Clamp(branchCount, 1, 1); // 主節點固定為1個分支
        
        if (Application.isPlaying)
        {
            EnsureInputPoint();
            EnsureOutputPoint();
        }
    }
}