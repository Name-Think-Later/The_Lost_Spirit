using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

/// <summary>
/// 簡單的UI連接線組件，使用Image來顯示連接線
/// </summary>
public class SimpleConnectionLine : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rectTransform;
    private Image image;
    
    [Header("線條設置")]
    public Color lineColor = Color.white;
    public float lineWidth = 4f;
    
    [Header("點擊中斷設置")]
    public Color hoverColor = Color.red;
    private Color originalColor;
    private bool isHovered = false;
    
    public NodeConnectionPoint StartPoint { get; private set; }
    public NodeConnectionPoint EndPoint { get; private set; }
    
    // 關聯的連接資料
    private NodeConnection associatedConnection;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        
        if (rectTransform == null)
            rectTransform = gameObject.AddComponent<RectTransform>();
            
        if (image == null)
        {
            image = gameObject.AddComponent<Image>();
            image.color = lineColor;
            // 啟用 raycast，允許點擊連線來中斷
            image.raycastTarget = true;
        }
        
        // 設置錨點為中心
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }
    
    /// <summary>
    /// 設置連接的兩個點
    /// </summary>
    public void SetConnection(NodeConnectionPoint startPoint, NodeConnectionPoint endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        UpdateLine();
    }
    
    /// <summary>
    /// 設置關聯的連接資料
    /// </summary>
    public void SetAssociatedConnection(NodeConnection connection)
    {
        associatedConnection = connection;
    }
    
    /// <summary>
    /// 更新線條位置和旋轉
    /// </summary>
    public void UpdateLine()
    {
        if (StartPoint == null || EndPoint == null || rectTransform == null) return;
        
        Vector3 startWorldPos = StartPoint.GetWorldPosition();
        Vector3 endWorldPos = EndPoint.GetWorldPosition();
        
        // 轉換為Canvas本地坐標
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            RectTransform canvasRT = canvas.transform as RectTransform;
            Vector2 startLocal, endLocal;
            
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                // World Space Canvas：直接轉換世界坐標到本地坐標
                startLocal = canvasRT.InverseTransformPoint(startWorldPos);
                endLocal = canvasRT.InverseTransformPoint(endWorldPos);
            }
            else
            {
                // Screen Space Canvas：使用 RectTransformUtility
                Camera cam = canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRT, 
                    RectTransformUtility.WorldToScreenPoint(cam, startWorldPos), 
                    cam, 
                    out startLocal);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRT, 
                    RectTransformUtility.WorldToScreenPoint(cam, endWorldPos), 
                    cam, 
                    out endLocal);
            }
            
            // 計算中心點和方向
            Vector2 center = (startLocal + endLocal) * 0.5f;
            Vector2 direction = endLocal - startLocal;
            float distance = direction.magnitude;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // 設置位置、尺寸和旋轉
            rectTransform.anchoredPosition = center;
            rectTransform.sizeDelta = new Vector2(distance, lineWidth);
            rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    
    void Update()
    {
        // 每幀更新線條位置（可以優化為只在需要時更新）
        if (StartPoint != null && EndPoint != null)
        {
            UpdateLine();
        }
        else if (manualStartPos != Vector3.zero || manualEndPos != Vector3.zero)
        {
            UpdateManualLine();
        }
    }
    
    // 手動位置變量（用於預覽線條）
    private Vector3 manualStartPos;
    private Vector3 manualEndPos;
    
    /// <summary>
    /// 手動設置起始和結束位置（用於預覽線條）
    /// </summary>
    public void SetPositions(Vector3 startPos, Vector3 endPos)
    {
        manualStartPos = startPos;
        manualEndPos = endPos;
        UpdateManualLine();
    }
    
    /// <summary>
    /// 使用手動位置更新線條
    /// </summary>
    private void UpdateManualLine()
    {
        if (rectTransform == null) return;
        
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            RectTransform canvasRT = canvas.transform as RectTransform;
            Vector2 startLocal, endLocal;
            
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                // World Space Canvas：直接轉換世界坐標到本地坐標
                startLocal = canvasRT.InverseTransformPoint(manualStartPos);
                endLocal = canvasRT.InverseTransformPoint(manualEndPos);
            }
            else
            {
                // Screen Space Canvas：使用 RectTransformUtility
                Camera cam = canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRT, 
                    RectTransformUtility.WorldToScreenPoint(cam, manualStartPos), 
                    cam, 
                    out startLocal);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRT, 
                    RectTransformUtility.WorldToScreenPoint(cam, manualEndPos), 
                    cam, 
                    out endLocal);
            }
            
            // 計算中心點和方向
            Vector2 center = (startLocal + endLocal) * 0.5f;
            Vector2 direction = endLocal - startLocal;
            float distance = direction.magnitude;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // 設置位置、尺寸和旋轉
            rectTransform.anchoredPosition = center;
            rectTransform.sizeDelta = new Vector2(distance, lineWidth);
            rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    
    /// <summary>
    /// 設置線條顏色
    /// </summary>
    public void SetColor(Color color)
    {
        lineColor = color;
        originalColor = color;
        if (image != null && !isHovered)
            image.color = color;
    }
    
    /// <summary>
    /// 設置線條寬度
    /// </summary>
    public void SetWidth(float width)
    {
        lineWidth = width;
        UpdateLine();
    }
    
    /// <summary>
    /// 處理滑鼠點擊事件
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 左鍵點擊中斷連線
            DisconnectLine();
        }
    }
    
    /// <summary>
    /// 中斷連線
    /// </summary>
    private void DisconnectLine()
    {
        NodeConnectionManager connectionManager = FindObjectOfType<NodeConnectionManager>();
        if (connectionManager != null && associatedConnection != null)
        {
            Debug.Log($"Disconnecting line between {StartPoint?.ParentNode?.name} and {EndPoint?.ParentNode?.name}");
            connectionManager.RemoveConnection(associatedConnection);
        }
        else if (connectionManager != null && StartPoint != null && EndPoint != null)
        {
            // 如果沒有關聯連接，嘗試根據連接點尋找連接
            var outputPoint = StartPoint.connectionType == ConnectionPointType.Output ? StartPoint : EndPoint;
            var inputPoint = StartPoint.connectionType == ConnectionPointType.Input ? StartPoint : EndPoint;
            
            foreach (var connection in outputPoint.Connections.ToArray())
            {
                if (connection.OutputPoint == outputPoint && connection.InputPoint == inputPoint)
                {
                    connectionManager.RemoveConnection(connection);
                    break;
                }
            }
        }
    }
}